using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Extension;
using SharedApp.Models;

namespace Client.App.Pages
{
    public partial class CartSummary : ComponentBase
    {
        [Inject] public IHttpClientHelperService HttpClientHelperService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public ICatalogMusicService CatalogMusicService { get; set; }
        [Inject] private IShopCartService ShopCartService { get; set; }
        private List<ShopCartWrapper> ShopCarts { get; set; } = new();
        private List<MusicCatalog> MusicCatalogs { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            ShopCarts = await ShopCartService.GetShopCart();
            MusicCatalogs = await CatalogMusicService.GetAsync();
            MusicCatalogs = MusicCatalogs.Take(10).ToList();
            await base.OnInitializedAsync();
        }
        private async void CreateCheckoutSesion()
        {
            var shopCarts = await ShopCartService.GetShopCart();

            //await HttpClientHelperService.Post<string>("Checkout", data);
            var data = shopCarts
                .Select((value, index) => new
                {
                    Key = value.IsMusicCatalog ? $"music-{index}" : $"audio-{index}",
                    Value = value.Id.ToString()
                })
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            var url = await HttpClientHelperService.Post("Checkout", data);
            NavigationManager.NavigateTo(url);
        }
    }
}