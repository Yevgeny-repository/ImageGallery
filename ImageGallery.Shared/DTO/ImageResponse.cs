namespace ImageGallery.Shared.DTO
{
    public class ImageResponse
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Url { get; set; }
        public string Download_url { get; set; }
        public string Friendly_image_url { get; set; }
        public int IsShown { get; set; }
    }
}