namespace Client.App.Pages
{
    public partial class CatalogAudioCreate : ComponentBase
    {
        [Inject] public IToastService ToastService { get; set; }
        [Inject] public IAudioCatalogService AudioCatalogService { get; set; }
        [Inject] public IProductService ProductService { get; set; }

        private AudioCatalog NewAudioCatalog { get; set; } = new();
        private EditContext _newContextAudioCatalog;
        private bool IsLoading { get; set; }
        private bool IsAnyImage { get; set; }

        public delegate void CatalogAudioInsertOnDb(int idCatalogAudio);
        public static event CatalogAudioInsertOnDb OnCompleteInsert;

        protected override void OnInitialized()
        {
            IsLoading = true;
            _newContextAudioCatalog = new EditContext(NewAudioCatalog);
            IsLoading = false;
            StateHasChanged();
        }
        private async void CreateCataloAudios()
        {
            try
            {
                if (!_newContextAudioCatalog.Validate()) return;

                NewAudioCatalog = await AudioCatalogService.CreateAsync(NewAudioCatalog);
                OnCompleteInsert.Invoke(NewAudioCatalog.Id);
                await ProductService.CreateCatalogOnStripeAsync(NewAudioCatalog!);
                NewAudioCatalog = new AudioCatalog();
                ToastService.ShowToast(ToastLevel.Success, $"Exito se creo {NewAudioCatalog!.Name}-{NewAudioCatalog.Brand} en el catalogo");
                StateHasChanged();
            }
            catch (Exception exception)
            {
                ToastService.ShowToast(ToastLevel.Error, exception.Message);
            }
        }
        public void SetIsAnyImageForCatalog(bool value)
        {
            IsAnyImage = value;
            StateHasChanged();
        }
    }
}