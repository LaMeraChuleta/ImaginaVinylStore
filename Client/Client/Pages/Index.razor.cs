using Microsoft.AspNetCore.Components;
using SharedApp.Models;
using System.Net.Http.Json;

namespace Client.App.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject] public HttpClient _Http { get; set; }
        [Inject] public IHttpClientFactory _HttpFactory { get; set; }
        private List<MusicCatalog> CatalogMusics { get; set; } = new();
        public Index() { }
        protected override async Task OnInitializedAsync()
        {
            _Http = _HttpFactory.CreateClient("CatalogMusic.API");
            CatalogMusics = await _Http.GetFromJsonAsync<List<MusicCatalog>>("MusicCatalog");
        }
    }
}
