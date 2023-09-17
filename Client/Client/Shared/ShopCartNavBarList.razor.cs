using Blazored.Toast.Services;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;

namespace Client.App.Shared
{
    public partial class ShopCartNavBarList : ComponentBase
    {
        [Inject] private IShopCartService ShopCartService { get; set; }
        [Inject] private NavigationManager? _navigationManager { get; set; }
        [Inject] public IToastService ToastService { get; set; }
        private List<MusicCatalog> MusicCatalogs { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            MusicCatalogs = await ShopCartService.GetShopCartToMusicCatalog();
            await base.OnInitializedAsync();
        }

        private async void DeleteShopCartItem(int idCatalogMusic)
        {
            try
            {
                var deleteItem = await ShopCartService.DeleteShopCartItem(idCatalogMusic);
                if (deleteItem)
                {
                    MusicCatalogs = await ShopCartService.GetShopCartToMusicCatalog();
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
    }
}
