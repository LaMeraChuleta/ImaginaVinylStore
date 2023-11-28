using SharedApp.Models;

namespace SharedApp.Extension
{
    public class ShopCartWrapper
    {
        public Guid Guid { get; set; }
        public MusicCatalog? CatalogMusic { get; set; }
        public AudioCatalog? AudioCatalog { get; set; }
        public int Id { get; set; }
        public string? Url { get; set; }
        public bool IsMusicCatalog { get; set; }
        public ShopCartWrapper() { }
        public ShopCartWrapper(MusicCatalog musicCatalog)
        {
            Guid = Guid.NewGuid();
            CatalogMusic = musicCatalog;
            Id = musicCatalog.Id;
            IsMusicCatalog = true;

            if (musicCatalog.Images is not null && musicCatalog.Images.Any())
                Url = musicCatalog.Images.First().Url;
        }
        public ShopCartWrapper(AudioCatalog audioCatalog)
        {
            Guid = Guid.NewGuid();
            AudioCatalog = audioCatalog;
            Id = audioCatalog.Id;

            if (audioCatalog.Images is not null && audioCatalog.Images.Any())
                Url = audioCatalog.Images.First().Url;
        }
        public decimal GetPrice()
        {
            return IsMusicCatalog
                ? CatalogMusic.Price
                : AudioCatalog.Price;
        }
    }
}