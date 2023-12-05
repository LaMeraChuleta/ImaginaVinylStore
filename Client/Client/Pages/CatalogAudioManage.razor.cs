namespace Client.App.Pages
{
    public partial class CatalogAudioManage : ComponentBase
    {
        [Inject] public IToastService ToastService { get; set; }
        [Inject] public IAudioCatalogService AudioCatalogService { get; set; }
        [Inject] public IProductService ProductService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        private List<AudioCatalog> AudioCatalogs { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            AudioCatalogs = await AudioCatalogService.GetAsync();
        }

        private async Task ActiveCatalogMusicInStripe(AudioCatalog audioCatalog)
        {
            try
            {
                await ProductService.CreateCatalogOnStripeAsync(audioCatalog);
            }
            catch (Exception exception)
            {
                ToastService.ShowToast(ToastLevel.Error, exception.Message);
            }
        }
        private void EditAudioCatalog(int id)
        {
            NavigationManager.NavigateTo($"EditCatalogAudio/{id}");
        }
    }
}
