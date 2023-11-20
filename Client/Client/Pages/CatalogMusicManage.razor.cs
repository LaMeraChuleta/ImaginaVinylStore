using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;
using static Client.App.Services.CatalogMusicService;

namespace Client.App.Pages
{
    public partial class CatalogMusicManage : ComponentBase
    {
        [Inject] public ICatalogMusicService CatalogMusicService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        private List<MusicCatalog> CatalogMusics { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            CatalogMusics = await CatalogMusicService.GetAsync(new FilterForCatalogMusic { IsActiveInStripe = null });
        }

        private void EditMusicCatalog(int id)
        {
            NavigationManager.NavigateTo($"EditCatalogMusic/{id}");
        }
    }
}
