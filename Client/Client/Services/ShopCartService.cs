using Blazored.LocalStorage;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using SharedApp.Models;

namespace Client.App.Services
{
    public class ShopCartService : IShopCartService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly ILocalStorageService _localStorageService;
        private readonly IShopCartNotificationService _shopCartNotificationService;

        public ShopCartService(
            AuthenticationStateProvider authenticationStateProvider, 
            IHttpClientHelper httpClientHelper, 
            ILocalStorageService localStorageService,
            IShopCartNotificationService shopCartNotificationService)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _localStorageService = localStorageService;
            _shopCartNotificationService = shopCartNotificationService;
            _httpClientHelper = httpClientHelper;
        }
        public async Task<List<ShopCart>> GetShopCart()
        {
            try
            {
                if (await SearchInServer())
                {
                    var fullShopCart = await _httpClientHelper.Get<List<ShopCart>>(nameof(ShopCart));                    
                    return fullShopCart.ToList() ?? new List<ShopCart>();
                }
                else
                {
                    return await _localStorageService.GetItemAsync<List<ShopCart>>(nameof(ShopCart)) ?? new();                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }        
        public async Task<int> GetShopCartCount()
        {
            try
            {                                
                var shopCarts = await GetShopCart(); 
                return shopCarts.Count();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> SetShopCartItem(ShopCart shopCart, MusicCatalog musicCatalog)
        {
            try
            {
                if (await SearchInServer())
                {                    
                    var fullShopCart = await _httpClientHelper.Get<List<ShopCart>>(nameof(ShopCart));
                    if (fullShopCart.Exists(x => x.MusicCatalogId == shopCart.MusicCatalogId)) return false;
                    else
                    {
                        var result = await _httpClientHelper.Post(nameof(ShopCart), shopCart);
                        fullShopCart.Add(result);
                        _shopCartNotificationService.NotifitShopCartCountChanges(fullShopCart.Count);
                        return true;
                    }
                }
                else
                {
                    var shopCartItems = await _localStorageService.GetItemAsync<List<ShopCart>>(nameof(ShopCart)) ?? new();
                    if (shopCartItems.Exists(x => x.MusicCatalogId == musicCatalog.Id)) return false;
                    else
                    {
                        shopCartItems.Add(shopCart);
                        await _localStorageService.SetItemAsync(nameof(ShopCart), shopCartItems);
                        _shopCartNotificationService.NotifitShopCartCountChanges(shopCartItems.Count);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<bool> SearchInServer()
        {
            var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authenticationState.User.Identity is { IsAuthenticated: true };
        }
    }
}
