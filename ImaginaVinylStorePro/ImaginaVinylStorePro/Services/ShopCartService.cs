using Blazored.LocalStorage;
using ImaginaVinylStorePro.Extension;
using ImaginaVinylStorePro.Interfaces;
using SharedApp.Models;

namespace ImaginaVinylStorePro.Services
{
    public class ShopCartService : IShopCartService
    {        
        private readonly ILocalStorageService _localStorageService;
        private readonly IShopCartNotificationService _shopCartNotificationService;

        public ShopCartService(            
            ILocalStorageService localStorageService,
            IShopCartNotificationService shopCartNotificationService)
        {
            _localStorageService = localStorageService;
            _shopCartNotificationService = shopCartNotificationService;            
        }

        public async Task<List<ShopCartWrapper>> GetShopCart()
        {
            try
            {
                return await _localStorageService.GetItemAsync<List<ShopCartWrapper>>(nameof(ShopCartWrapper)) ?? new();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int[]> GetShopCartId()
        {
            try
            {
                var data = await _localStorageService.GetItemAsync<List<ShopCartWrapper>>(nameof(ShopCartWrapper)) ?? new();
                return data.Select(x => x.Id).ToArray();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> GetShopCartCount()
        {
            try
            {
                var shopCarts = await GetShopCart();
                return shopCarts.Count;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> SetShopCartItem(AudioCatalog audioCatalog)
        {
            try
            {
                var shopCartItems = await _localStorageService.GetItemAsync<List<ShopCartWrapper>>(nameof(ShopCartWrapper)) ?? new();
                if (shopCartItems.Exists(x => x.Id == audioCatalog.Id))
                {
                    return false;
                }
                else
                {
                    shopCartItems.Add(new ShopCartWrapper(audioCatalog));
                    await _localStorageService.SetItemAsync(nameof(ShopCartWrapper), shopCartItems);
                    _shopCartNotificationService.NotifitShopCartCountChanges(shopCartItems.Count);
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> SetShopCartItem(MusicCatalog musicCatalog)
        {
            try
            {
                var shopCartItems = await _localStorageService.GetItemAsync<List<ShopCartWrapper>>(nameof(ShopCartWrapper)) ?? new();
                if (shopCartItems.Exists(x => x.Id == musicCatalog.Id))
                {
                    return false;
                }
                else
                {
                    shopCartItems.Add(new ShopCartWrapper(musicCatalog));
                    await _localStorageService.SetItemAsync(nameof(ShopCartWrapper), shopCartItems);
                    _shopCartNotificationService.NotifitShopCartCountChanges(shopCartItems.Count);
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteShopCartItem(Guid guid)
        {
            try
            {
                var shopCarts = await GetShopCart();
                var countRemove = shopCarts.RemoveAll(x => x.Guid == guid);
                await _localStorageService.SetItemAsync(nameof(ShopCartWrapper), shopCarts);
                _shopCartNotificationService.NotifitShopCartCountChanges(shopCarts.Count);
                return countRemove == 1;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteShopCart()
        {
            try
            {               
                await _localStorageService.SetItemAsync(nameof(ShopCartWrapper), new List<ShopCartWrapper>());
                _shopCartNotificationService.NotifitShopCartCountChanges(0);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}