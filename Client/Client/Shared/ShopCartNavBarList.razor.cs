using Blazored.Toast.Services;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;

namespace Client.App.Shared
{
    public partial class ShopCartNavBarList : ComponentBase
    {
        [Parameter] public EventCallback SendCloseComponent { get; set; }
        [Inject] private IShopCartService ShopCartService { get; set; }
        [Inject] private NavigationManager? _navigationManager { get; set; }
        [Inject] public IToastService ToastService { get; set; }
        private List<MusicCatalog> MusicCatalogs { get; set; } = new();
        private bool IsLoading { get; set; } = true;
        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            MusicCatalogs = await ShopCartService.GetShopCart();
            IsLoading = false;
            await base.OnInitializedAsync();
        }

        private async void DeleteShopCartItem(int idCatalogMusic)
        {
            try
            {
                var deleteItem = await ShopCartService.DeleteShopCartItem(idCatalogMusic);                
                if (deleteItem)
                {
                    MusicCatalogs = await ShopCartService.GetShopCart();
                    ToastService.ShowToast(ToastLevel.Success, $"Se elimino el producto del carrito");
                    StateHasChanged();
                }
                else
                {
                    ToastService.ShowToast(ToastLevel.Warning, $"No se pudo eliminar el producto del carrito");
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
            _navigationManager?.NavigateTo(" CartSummary");
        }
    }
}
