namespace FileManager.Transform.Models
{
    public class Photo
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? AltText { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }        
    }
}
