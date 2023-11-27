using SharedApp.Models;

namespace SharedApp.Extension
{
    public class ShopCartWrapper
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int Price { get; set; }
        public string? Url { get; set; }
        public string? Format { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsMusicCatalog { get; set; }
        public ShopCartWrapper() { }
        public ShopCartWrapper(MusicCatalog musicCatalog)
        {
            Guid = Guid.NewGuid();
            Id = musicCatalog.Id;
            Price = musicCatalog.Price;
            Name = musicCatalog.Title;
            IsMusicCatalog = true;

            if (musicCatalog.Format is not null)
                Format = musicCatalog.Format.Name;
            if (musicCatalog.Images is not null && musicCatalog.Images.Any())
                Url = musicCatalog.Images.First().Url;
        }
        public ShopCartWrapper(AudioCatalog audioCatalog)
        {
            Guid = Guid.NewGuid();
            Id = audioCatalog.Id;
            Price = audioCatalog.Price;
            Name = audioCatalog.Name + "-" + audioCatalog.Brand;
            IsMusicCatalog = false;

            if (audioCatalog.Images is not null && audioCatalog.Images.Any())
                Url = audioCatalog.Images.First().Url;
        }
    }
}