namespace Client.App.Shared
{
    public partial class ShopCartNavBarList : ComponentBase
    {
        [Parameter] public EventCallback SendCloseComponent { get; set; }
        [Inject] private IShopCartService ShopCartService { get; set; }
        [Inject] private NavigationManager? NavigationManager { get; set; }
        [Inject] public IToastService ToastService { get; set; }
        private List<ShopCartWrapper> ShopCarts { get; set; }
        private bool IsLoading { get; set; } = true;
        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            ShopCarts = await ShopCartService.GetShopCart();
            IsLoading = false;
            await base.OnInitializedAsync();
        }

        private async void DeleteShopCartItem(Guid guid)
        {
            try
            {
                var deleteItem = await ShopCartService.DeleteShopCartItem(guid);
                if (deleteItem)
                {
                    ShopCarts = await ShopCartService.GetShopCart();
                    ToastService.ShowToast(ToastLevel.Success, "Se elimino el producto del carrito");
                    StateHasChanged();
                }
                else
                {
                    ToastService.ShowToast(ToastLevel.Warning, "No se pudo eliminar el producto del carrito");
                }
            }
            catch (Exception exception)
            {
                ToastService.ShowToast(ToastLevel.Error, exception.Message);
            }
        }

        private async void NavigateToCartSummary()
        {
            await SendCloseComponent.InvokeAsync();
            NavigationManager?.NavigateTo(" CartSummary");
        }
    }
}
