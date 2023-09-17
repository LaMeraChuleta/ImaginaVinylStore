using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;

namespace Client.App.Pages
{
    public partial class CartSummary : ComponentBase
    {
        [Inject] public IHttpClientHelperService HttpClientHelperService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] private IShopCartService _shopCartService { get; set; }
        [Inject] public ICatalogMusicService CatalogMusicService { get; set; }
        [Inject] private IShopCartService ShopCartService { get; set; }
        private List<MusicCatalog> MusicCatalogsInShopCart { get; set; } = new();
        private List<MusicCatalog> MusicCatalogs { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            MusicCatalogsInShopCart = await ShopCartService.GetShopCartToMusicCatalog();
            MusicCatalogs = await CatalogMusicService.GetAsync();
            MusicCatalogs = MusicCatalogs.Take(10).ToList();
            await base.OnInitializedAsync();
        }
        private async void TestStrape()
        {
            var url = await HttpClientHelperService.Get<string>("ShopCart/Strapi");
            NavigationManager.NavigateTo(url);

        }
    }
}
