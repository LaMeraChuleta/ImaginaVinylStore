namespace Client.App.Pages.Audio
{
    public partial class Detail : ComponentBase
    {
        [Parameter] public int IdAudioCatalog { get; set; }
        [Inject] public IAudioCatalogService CatalogAudioService { get; set; }
        [Inject] public IToastService ToastService { get; set; }
        [Inject] public IShopCartService ShopCartService { get; set; }

        private AudioCatalog AudioCatalog { get; set; }        
        protected override async Task OnInitializedAsync()
        {
            try
            {
                AudioCatalog = await CatalogAudioService.GetByIdAsync(IdAudioCatalog);                
            }
            catch (Exception ex)
            {
                ToastService.ShowToast(ToastLevel.Error, ex.Message);
            }

            await base.OnInitializedAsync();
        }

        private async void AddItemShopCart()
        {
            try
            {
                var isItemCreate = await ShopCartService.SetShopCartItem(AudioCatalog);
                if (isItemCreate)
                {
                    ToastService.ShowSuccess($"Se guardo este articulo en el carrito. {AudioCatalog.Name}-{AudioCatalog.Brand}");
                }
                else
                {
                    ToastService.ShowWarning($"Ya existe este articulo en el carrito. {AudioCatalog.Name}-{AudioCatalog.Brand}");
                }
            }
            catch (Exception exception)
            {
                ToastService.ShowToast(ToastLevel.Error, exception.Message);
            }
        }
    }
}
