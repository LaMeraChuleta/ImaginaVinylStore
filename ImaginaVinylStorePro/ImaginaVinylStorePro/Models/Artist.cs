using System.ComponentModel.DataAnnotations;

namespace SharedApp.Models;

public class Artist
{
    public int Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Country { get; set; }
    public ImageArtist? Image { get; set; }
    public ICollection<MusicCatalog> CatalogMusics { get; } = new List<MusicCatalog>();
}