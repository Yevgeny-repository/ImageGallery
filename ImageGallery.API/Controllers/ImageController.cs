using EnsureThat;
using ImageGallery.BL;
using ImageGallery.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageGallery.API.Controllers
{
    [Route("api/images/")]
    public class ImageController : ControllerBase
    {
        private readonly ILogger<ImageController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService,
                                ILogger<ImageController> logger,
                                     IConfiguration configuration)
        {
            Ensure.That(logger, nameof(logger)).IsNotNull();
            Ensure.That(configuration, nameof(configuration)).IsNotNull();
            Ensure.That(imageService, nameof(imageService)).IsNotNull();

            _logger = logger;
            _configuration = configuration;
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<IList<ImageResponse>> Get()
        {
            string url = _configuration.GetValue<string>("appSettings:ImagesApiUrl");
            return await _imageService.GetRandomImages(url);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var b = await _imageService.GetImage(id);
            return File(b, "image/jpeg");
        }
    }
}