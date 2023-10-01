using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;

namespace Client.App.Pages
{
    public partial class CatalogMusicManage : ComponentBase
    {
        [Inject] public ICatalogMusicService CatalogMusicService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        private List<MusicCatalog> CatalogMusics { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            CatalogMusics = await CatalogMusicService.GetAsync();
        }

        private void EditMusicCatalog(int id)
        {
            NavigationManager.NavigateTo($"EditCatalogMusic/{id}");
        }
    }
}
