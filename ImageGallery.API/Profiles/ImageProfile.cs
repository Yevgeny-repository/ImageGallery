using ImageGallery.Entities.Models;
using ImageGallery.Shared.DTO;

namespace ImageGallery.API.Profiles
{
    public class ImageProfile : AutoMapper.Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, ImageResponse>();
        }
    }
}