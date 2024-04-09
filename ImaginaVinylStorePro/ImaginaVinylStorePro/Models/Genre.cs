using System.ComponentModel.DataAnnotations;

namespace SharedApp.Models;

public class Genre
{
    public int Id { get; set; }
    [Required] public string Name { get; set; }
    public ICollection<MusicCatalog> CatalogMusics { get; } = new List<MusicCatalog>();
}