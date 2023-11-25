using SharedApp.Models;

namespace Client.App.Interfaces
{
    public interface IProductService
    {
        Task<MusicCatalog> CreateCatalogOnStripeAsync(MusicCatalog musicCatalog);
        Task<AudioCatalog> CreateAsync(AudioCatalog audioCatalog);
    }
}
