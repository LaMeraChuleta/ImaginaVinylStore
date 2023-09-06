using SharedApp.Models;

namespace Client.App.Interfaces
{
    public interface IShopCartService
    {
        Task<List<ShopCart>> GetShopCart();
        Task<int> GetShopCartCount();
        Task<bool> SetShopCartItem(ShopCart shopCart, MusicCatalog musicCatalog);
    }
}
