namespace SharedApp.Models;

public class ShopCart
{
    public int Id { get; set; }
    public string? ApplicationUserId { get; set; }
    public int? MusicCatalogId { get; set; }
    public int? AudioCatalogId { get; set; }
    public int Amount { get; set; }
    public int UnitPrice { get; set; }
    public MusicCatalog? CatalogMusic { get; set; }
    public AudioCatalog? AudioCatalog { get; set; }
}