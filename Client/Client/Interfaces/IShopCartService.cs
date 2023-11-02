using SharedApp.Models;

namespace Client.App.Interfaces
{
    public interface IShopCartService
    {
        Task<List<MusicCatalog>> GetShopCart();        
        Task<int> GetShopCartCount();
        Task<bool> SetShopCartItem(MusicCatalog musicCatalog);
        Task<bool> DeleteShopCartItem(int idCatalogMusic);
    }
}
