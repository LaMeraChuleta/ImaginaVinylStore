using ImaginaVinylStorePro.Validation;
using System.ComponentModel.DataAnnotations;

namespace SharedApp.Models;

public class Presentation
{
    public int Id { get; set; }
    [Required] public string? Name { get; set; }
    [NotZero] public int FormatId { get; set; }
    public Format? Format { get; set; }
    public ICollection<MusicCatalog> CatalogMusics { get; } = new List<MusicCatalog>();
}