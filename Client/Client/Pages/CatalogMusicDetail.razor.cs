using Blazored.Toast.Services;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;

namespace Client.App.Pages;

public partial class CatalogMusicDetail : ComponentBase
{
    [Parameter] public int IdMusicCatalog { get; set; }
    [Inject] public ICatalogMusicService CatalogMusicService { get; set; }
    [Inject] public IToastService ToastService { get; set; }
    [Inject] public IShopCartService ShopCartService { get; set; }

    private MusicCatalog MusicCatalog { get; set; }
    private List<MusicCatalog> CatalogMusics { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            MusicCatalog = await CatalogMusicService.GetByIdAsync(IdMusicCatalog);
            CatalogMusics = (await CatalogMusicService.GetAsync()).Take(20).ToList();
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
            var isItemCreate = await ShopCartService.SetShopCartItem(MusicCatalog);
            if (isItemCreate)
            {
                ToastService.ShowSuccess($"Se guardo este articulo en el carrito. {MusicCatalog.Title}-{MusicCatalog.Artist?.Name}");
            }
            else
            {
                ToastService.ShowWarning($"Ya existe este articulo en el carrito. {MusicCatalog.Title}-{MusicCatalog.Artist?.Name}");
            }
        }
        catch (Exception exception)
        {
            ToastService.ShowToast(ToastLevel.Error, exception.Message);
        }
    }
}