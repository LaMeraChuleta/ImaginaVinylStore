namespace Client.App.Pages.Audio
{
    public partial class Edit : ComponentBase
    {
        [Parameter] public int AudioCatalogId { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IToastService ToastService { get; set; }
        [Inject] public IAudioCatalogService AudioCatalogService { get; set; }
        [Inject] public IProductService ProductService { get; set; }

        private AudioCatalog EditAudioCatalog { get; set; } = new();
        private EditContext _editContextAudioCatalog;
        private bool IsLoading { get; set; }
        private bool IsAnyImage { get; set; }

        public delegate void CatalogAudioEditOnDb(int idCatalogAudio);
        public static event CatalogAudioEditOnDb OnCompleteEdit;

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            EditAudioCatalog = await AudioCatalogService.GetByIdAsync(AudioCatalogId);
            _editContextAudioCatalog = new EditContext(EditAudioCatalog);
            IsLoading = false;
            StateHasChanged();
        }
        private async void EditCatalogAudio()
        {
            try
            {
                if (!_editContextAudioCatalog.Validate()) return;

                if (!await AudioCatalogService.UpdateAsync(EditAudioCatalog))
                {
                    ToastService.ShowToast(ToastLevel.Success, $"No se pudo actualizar {EditAudioCatalog.Name}-{EditAudioCatalog.Brand} en el catalogo");
                }

                OnCompleteEdit.Invoke(EditAudioCatalog.Id);
                ToastService.ShowToast(ToastLevel.Success, $"Exito se actualizo {EditAudioCatalog.Name}-{EditAudioCatalog.Brand} en el catalogo");
                StateHasChanged();
                NavigationManager.NavigateTo("/ManageCatalogAudio");
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