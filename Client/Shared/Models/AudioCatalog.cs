namespace SharedApp.Models
{
    public class AudioCatalog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public ICollection<ProductCatalog> Product { get; set; }
    }
}