namespace SharedApp.Models
{
    public class MusicCatalog
    {
        public int Id { get; set; }
        public IEnumerable<ImageCatalog>? Images { get; set; }
        public string Title { get; set; }
        public Artist? Artist { get; set; }
        public int ArtistId { get; set; }
        public Genre? Genre { get; set; }
        public int GenreId { get; set; }
        public Format? Format { get; set; }
        public int FormatId { get; set; }
        public Presentation? Presentation { get; set; }
        public int PresentationId { get; set; }
        public string Country { get; set; }
        public int Year { get; set; }
        public int StatusCover { get; set; }
        public int StatusGeneral { get; set; }
        public int Price { get; set; }
        public string Matrix { get; set; }
        public string Label { get; set; }
        public DateTime CreateAt { get; set; }
    }
}


