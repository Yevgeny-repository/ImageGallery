using ImageGallery.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageGallery.BL
{
    public interface IImageService
    {
        Task<IList<ImageResponse>> GetRandomImages(string apiUrl);

        Task<Byte[]> GetImage(int id);
    }
}