namespace ImageGallery.Entities.Models
{
    public class Image
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Url { get; set; }
        public string Download_url { get; set; }
    }
}