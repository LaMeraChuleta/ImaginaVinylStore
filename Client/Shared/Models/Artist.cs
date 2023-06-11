using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SharedApp.Models;

public class Artist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public ICollection<MusicCatalog> CatalogMusics { get; } = new List<MusicCatalog>();
}