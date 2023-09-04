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

        public ShopCartService(AuthenticationStateProvider authenticationStateProvider, IHttpClientHelper httpClientHelper, ILocalStorageService localStorageService)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _localStorageService = localStorageService;
            _httpClientHelper = httpClientHelper;
        }
        public Task GetShopCart(int? id = null)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SetShopCartItem(ShopCart shopCart, MusicCatalog musicCatalog)
        {
            try
            {
                var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                if (authenticationState.User.Identity is { IsAuthenticated: true })
                {
                    var fullShopCart = await _httpClientHelper.Get<List<ShopCart>>(nameof(ShopCart));
                    if (fullShopCart.Exists(x => x.MusicCatalogId == shopCart.MusicCatalogId)) return false;
                    else
                    {
                        await _httpClientHelper.Post(nameof(ShopCart), shopCart);
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
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
