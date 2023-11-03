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
        private List<MusicCatalog> MusicCatalogsInShopCart { get; set; } = new();
        private List<MusicCatalog> MusicCatalogs { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            MusicCatalogsInShopCart = await ShopCartService.GetShopCart();
            MusicCatalogs = await CatalogMusicService.GetAsync();
            MusicCatalogs = MusicCatalogs.Take(10).ToList();
            await base.OnInitializedAsync();
        }
        private async void TestStrape()
        {
            var value = new ShopCart
            {
                MusicCatalogs = await ShopCartService.GetShopCart()
            };

            string url = await HttpClientHelperService.Post("Checkout", value, onlyString: true);
            NavigationManager.NavigateTo(url);
        }
    }
}