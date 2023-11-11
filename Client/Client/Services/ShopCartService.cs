using Blazored.LocalStorage;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using SharedApp.Models;

namespace Client.App.Services
{
    public class ShopCartService : IShopCartService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IHttpClientHelperService _httpClientHelper;
        private readonly ILocalStorageService _localStorageService;
        private readonly IShopCartNotificationService _shopCartNotificationService;

        public ShopCartService(
            AuthenticationStateProvider authenticationStateProvider,
            IHttpClientHelperService httpClientHelper,
            ILocalStorageService localStorageService,
            IShopCartNotificationService shopCartNotificationService)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _localStorageService = localStorageService;
            _shopCartNotificationService = shopCartNotificationService;
            _httpClientHelper = httpClientHelper;
        }

        public async Task<List<MusicCatalog>> GetShopCart()
        {
            try
            {
                return await _localStorageService.GetItemAsync<List<MusicCatalog>>(nameof(MusicCatalog)) ?? new();
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
                var data = await _localStorageService.GetItemAsync<List<MusicCatalog>>(nameof(MusicCatalog)) ?? new();
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
        public async Task<bool> SetShopCartItem(MusicCatalog musicCatalog)
        {
            try
            {
                var shopCartItems = await _localStorageService.GetItemAsync<List<MusicCatalog>>(nameof(MusicCatalog)) ?? new();
                if (shopCartItems.Exists(x => x.Id == musicCatalog.Id))
                {
                    return false;
                }
                else
                {
                    shopCartItems.Add(musicCatalog);
                    await _localStorageService.SetItemAsync(nameof(MusicCatalog), shopCartItems);
                    _shopCartNotificationService.NotifitShopCartCountChanges(shopCartItems.Count);
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteShopCartItem(int idCatalogMusic)
        {
            try
            {
                var shopCarts = await GetShopCart();
                var countRemove = shopCarts.RemoveAll(x => x.Id == idCatalogMusic);
                await _localStorageService.SetItemAsync(nameof(MusicCatalog), shopCarts);
                _shopCartNotificationService.NotifitShopCartCountChanges(shopCarts.Count);
                return countRemove == 1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}