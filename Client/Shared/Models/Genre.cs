namespace SharedApp.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
		public ICollection<CatalogMusic> CatalogMusics { get; } = new List<CatalogMusic>();
	}
}
