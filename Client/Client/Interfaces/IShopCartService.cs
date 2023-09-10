using SharedApp.Models;

namespace Client.App.Interfaces
{
    public interface IShopCartService
    {
        Task<List<ShopCart>> GetShopCart();
        Task<List<MusicCatalog>> GetShopCartToMusicCatalog();
        Task<int> GetShopCartCount();
        Task<bool> SetShopCartItem(ShopCart shopCart, MusicCatalog musicCatalog);
        Task<bool> DeleteShopCartItem(int idCatalogMusic);
    }
}
