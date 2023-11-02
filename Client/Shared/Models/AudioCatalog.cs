namespace SharedApp.Models
{
    public class AudioCatalog
    {
        // Primary key
        public int Id { get; set; }

        public int Price { get; set; }
        public bool ActiveInStripe { get; set; }
        public int? Discount { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string? IdProductStripe { get; set; }
        public string? IdPriceStripe { get; set; }

        // Foreign key  

        // Navigation properties
        public IEnumerable<ImageAudio>? Images { get; set; }        
    }
}