namespace SharedApp.Models
{
    public class AudioCatalog
    {
        public int Id { get; set; }
        public string? IdProductStripe { get; set; }
        public string? IdPriceStripe { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool ActiveInStripe { get; set; }
        public IEnumerable<ImageAudio>? Images { get; set; }
    }
}