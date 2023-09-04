using SharedApp.Models;

namespace Client.App.Interfaces
{
    public interface IShopCartService
    {
        Task GetShopCart(int? id = null);
        Task<bool> SetShopCartItem(ShopCart shopCart, MusicCatalog musicCatalog);
    }
}
