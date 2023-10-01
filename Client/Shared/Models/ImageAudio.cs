namespace SharedApp.Models
{
    public class ImageAudio
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public AudioCatalog? AudioCatalog { get; set; }
        public int AudioCatalogId { get; set; }
    }
}
