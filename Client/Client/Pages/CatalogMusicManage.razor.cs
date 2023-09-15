using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;

namespace Client.App.Pages
{
    public partial class CatalogMusicManage : ComponentBase
    {
        [Inject] public IHttpClientHelperService HttpClientHelper { get; set; }
        private List<MusicCatalog> CatalogMusics { get; set; } = new();
        protected override async Task OnParametersSetAsync()
        {
            CatalogMusics = await HttpClientHelper.Get<List<MusicCatalog>>(nameof(MusicCatalog));
        }
    }
}
