using AutoMapper;
using EnsureThat;
using ImageGallery.Core;
using ImageGallery.Entities.Models;
using ImageGallery.Shared.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.BL
{
    /// <summary>
    /// This class represent call external images api logic.
    /// </summary>
    public class ImageService : IImageService
    {
        #region Fields

        private IHttpGetRequestSender _sender;
        private IMemoryCache _cache;
        private IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string friendly_url_template = "";

        #endregion Fields

        #region Constructor

        public ImageService(IHttpGetRequestSender sender,
                            IHttpContextAccessor httpContextAccessor,
                            IMemoryCache cache,
                            IMapper mapper)
        {
            Ensure.That(sender, nameof(sender)).IsNotNull();
            _sender = sender;

            Ensure.That(cache, nameof(cache)).IsNotNull();
            _cache = cache;

            Ensure.That(mapper, nameof(mapper)).IsNotNull();
            _mapper = mapper;

            Ensure.That(httpContextAccessor, nameof(httpContextAccessor)).IsNotNull();
            _httpContextAccessor = httpContextAccessor;

            var api = $"api/images/";
            var request = _httpContextAccessor.HttpContext.Request;
            friendly_url_template = $"{request.Scheme}://{request.Host}/{api}";
        }

        #endregion Constructor

        #region Public methods

        /// <summary>
        /// Get 5 random elements
        /// Every image is unique and not seen before in previous invocations
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        public async Task<IList<ImageResponse>> GetRandomImages(string apiUrl)
        {
            var rnd = new Random();
            IList<ImageResponse> images = null;
            images = await ConvertToImageResponse(apiUrl);
            images = GetNotShownImages(images);

            var randomItems = images.OrderBy(x => rnd.Next()).Take(5).ToList();
            await SetShownImagesToCache(randomItems);
            return randomItems;
        }

        /// <summary>
        /// Get Image by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Byte[]> GetImage(int id)
        {
            IList<ImageResponse> model = null;
            string cacheKey = "images";
            Byte[] image = null;

            if (_cache.TryGetValue(cacheKey, out model))
            {
                string url = model.First(x => x.Id == id.ToString()).Download_url;
                var response = await _sender.GetAsync(url);
                image = await response.Content.ReadAsByteArrayAsync();
            }
            return image;
        }

        #endregion Public methods

        #region Private methods

        /// <summary>
        /// Convert images model to IList<ImageResponse>
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        private async Task<IList<ImageResponse>> ConvertToImageResponse(string apiUrl)
        {
            IList<ImageResponse> imagesResponse = null;
            string cacheKey = "images";

            if (!_cache.TryGetValue(cacheKey, out imagesResponse))
            {
                Image[] model = await GetImagesDataFromApi(apiUrl);

                imagesResponse = _mapper.Map<IList<ImageResponse>>(model);
                imagesResponse
                    .Select(s => { s.Friendly_image_url = friendly_url_template + s.Id; return s; }).ToArray();
                if (imagesResponse != null)
                {
                    _cache.Set(cacheKey, imagesResponse,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(60)));
                }
            }

            return imagesResponse;
        }

        /// <summary>
        /// Get images model from api
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        private async Task<Image[]> GetImagesDataFromApi(string apiUrl)
        {
            var response = await _sender.GetAsync(apiUrl);

            var stringContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Image[]>(stringContent);
        }

        /// <summary>
        /// Set 5 random images to cache
        /// </summary>
        /// <param name="newImages"></param>
        /// <returns></returns>
        private async Task<IList<ImageResponse>> SetShownImagesToCache(IList<ImageResponse> newImages)
        {
            string cacheKey = "shownImages";
            List<ImageResponse> imagesResponse;

            if (!_cache.TryGetValue(cacheKey, out imagesResponse))
            {
                imagesResponse = new List<ImageResponse>();
            }

            imagesResponse.AddRange(newImages);

            _cache.Set(cacheKey, imagesResponse,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(60)));

            return await Task.FromResult<IList<ImageResponse>>(imagesResponse);
        }

        /// <summary>
        /// This function will return images that not seen before in previous invocations
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        private IList<ImageResponse> GetNotShownImages(IList<ImageResponse> images)
        {
            List<ImageResponse> showImages;
            string cacheKey = "shownImages";
            if (!_cache.TryGetValue(cacheKey, out showImages))
            {
                return images;
            }

            var notShown = images.Except(showImages).ToList();
            if (notShown.Count == 0)
            {
                _cache.Remove(cacheKey);
                return images;
            }

            return notShown;
        }

        #endregion Private methods
    }
}