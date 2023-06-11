namespace SharedApp.Models
{
    public class ImageCatalog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public MusicCatalog MusicCatalog { get; set; }
        public int MusicCatalogId { get; set; }
    }
}