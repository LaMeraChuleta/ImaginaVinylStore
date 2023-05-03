namespace SharedApp.Models
{
    public class Presentation
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        //public Format? Format { get; set; }
        public int FormatId { get; set; }
        public ICollection<CatalogMusic> CatalogMusics { get; } = new List<CatalogMusic>();

    }
}
