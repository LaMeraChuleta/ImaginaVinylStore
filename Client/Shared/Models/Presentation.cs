namespace SharedApp.Models;

public class Presentation
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int FormatId { get; set; }
    public ICollection<MusicCatalog> CatalogMusics { get; } = new List<MusicCatalog>();
}