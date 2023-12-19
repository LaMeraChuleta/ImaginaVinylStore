using static Client.App.Services.CatalogMusicService;

namespace Client.App.Pages.Music
{
    public partial class Manage : ComponentBase
    {
        [Inject] public IToastService ToastService { get; set; }
        [Inject] public ICatalogMusicService CatalogMusicService { get; set; }
        [Inject] public IProductService ProductService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        private List<MusicCatalog> CatalogMusics { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            CatalogMusics = await CatalogMusicService.GetAsync(new FilterForCatalogMusic { IsActiveInStripe = null });
        }

        private async Task ActiveCatalogMusicInStripe(MusicCatalog musicCatalog)
        {
            try
            {
                await ProductService.CreateCatalogOnStripeAsync(musicCatalog);
            }
            catch (Exception exception)
            {
                ToastService.ShowToast(ToastLevel.Error, exception.Message);
            }
        }
        private void EditMusicCatalog(int id)
        {
            NavigationManager.NavigateTo($"EditCatalogMusic/{id}");
        }
    }
}
