using System.ComponentModel.DataAnnotations;

namespace SharedApp.Models;

public class MusicCatalog
{
    public int Id { get; set; }
    public IEnumerable<ImageCatalog>? Images { get; set; }
    [Required] public string Title { get; set; }
    public Artist? Artist { get; set; }
    [Required] public int ArtistId { get; set; }
    public Genre? Genre { get; set; }
    [Required] public int GenreId { get; set; }
    public Format? Format { get; set; }
    [Required] public int FormatId { get; set; }
    public Presentation? Presentation { get; set; }
    [Required] public int PresentationId { get; set; }
    [Required] public string Country { get; set; }
    [Required] public int Year { get; set; }
    [Range(0, 10)] public int StatusCover { get; set; }
    [Range(0, 10)] public int StatusGeneral { get; set; }
    public int Price { get; set; }
    [Required] public string Matrix { get; set; }
    [Required] public string Label { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;
}