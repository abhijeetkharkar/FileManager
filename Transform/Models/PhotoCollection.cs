namespace FileManager.Transform.Models
{
    public class PhotoCollection : Collection
    {
        public string? ThumbnailUrl { get; set; }
        public string? ThumbnailAltText { get; set; }
        public string? Description { get; set; }
        public bool? Eager { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public IEnumerable<Photo>? Photos { get; set; }
        public Collection? Previous { get; set; }
        public Collection? Next { get; set; }
    }

    public class Collection
    {
        public string? CollectionId { get; set; }
        public string? Title { get; set; }

    }
}
