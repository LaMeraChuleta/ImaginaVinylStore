using SharedApp.Validation;
using System.ComponentModel.DataAnnotations;

namespace SharedApp.Models;

public class MusicCatalog
{
    // Primary key
    public int Id { get; set; }

    [Required] public string Title { get; set; }
    [Required] public string Country { get; set; }
    [Required] public int Year { get; set; }
    [Range(0, 10)] public int StatusCover { get; set; }
    [Range(0, 10)] public int StatusGeneral { get; set; }
    [Required] public string Matrix { get; set; }
    [Required] public string Label { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public bool ActiveInStripe { get; set; }
    public int Price { get; set; }
    public int? Discount { get; set; }
    public string? IdProductStripe { get; set; }
    public string? IdPriceStripe { get; set; }

    // Foreign key  
    [NotZero] public int ArtistId { get; set; }
    [NotZero] public int GenreId { get; set; }
    [NotZero] public int FormatId { get; set; }
    [NotZero] public int? PresentationId { get; set; }

    // Navigation properties
    public Artist? Artist { get; set; }
    public Genre? Genre { get; set; }
    public Format? Format { get; set; }
    public Presentation? Presentation { get; set; }
    public IEnumerable<ImageCatalog>? Images { get; set; }
}