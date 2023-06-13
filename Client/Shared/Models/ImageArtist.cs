namespace SharedApp.Models;

public class ImageArtist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public Artist Artist { get; set; }
    public int ArtistId { get; set; }
}