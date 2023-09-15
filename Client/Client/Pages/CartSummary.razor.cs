using Client.App.Interfaces;
using Client.App.Services;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;

namespace Client.App.Pages
{    
    public partial class CartSummary : ComponentBase
    {
        [Inject] public IHttpClientHelper HttpClientHelper { get; set; }
        [Inject] private IShopCartService _shopCartService { get; set; }
        private List<MusicCatalog> MusicCatalogsInShopCart { get; set; } = new();
        private List<MusicCatalog> MusicCatalogs { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            MusicCatalogsInShopCart = await _shopCartService.GetShopCartToMusicCatalog();
            MusicCatalogs = await HttpClientHelper.Get<List<MusicCatalog>>(nameof(MusicCatalog));
            MusicCatalogs = MusicCatalogs.Take(10).ToList();
            await base.OnInitializedAsync();
        }
    }
}
