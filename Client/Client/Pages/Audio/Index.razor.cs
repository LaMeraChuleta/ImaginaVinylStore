namespace Client.App.Pages.Audio
{
    public partial class Index : ComponentBase
    {
        [Parameter] public string TypeFormat { get; set; }        
        [Inject] public IAudioCatalogService CatalogAudioService { get; set; }        
        [Inject] public IToastService ToastService { get; set; }
                        
        private List<AudioCatalog> AudioCatalogs { get; set; } = new();        
        private bool IsLoading { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                IsLoading = true;               
                AudioCatalogs = await CatalogAudioService.GetAsync();       
                IsLoading = false;
                StateHasChanged();

            }
            catch (Exception ex)
            {
                ToastService.ShowToast(ToastLevel.Error, ex.Message);
            }

            await base.OnParametersSetAsync();
        }   
    }
}