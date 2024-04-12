using ImaginaVinylStorePro.Extension;
using SharedApp.Models;

namespace ImaginaVinylStorePro.Interfaces
{
    public interface IShopCartService
    {
        Task<List<ShopCartWrapper>> GetShopCart();
        Task<int[]> GetShopCartId();
        Task<int> GetShopCartCount();
        Task<bool> SetShopCartItem(AudioCatalog audioCatalog);
        Task<bool> SetShopCartItem(MusicCatalog musicCatalog);
        Task<bool> DeleteShopCartItem(Guid guid);
        Task<bool> DeleteShopCart();
    }
}