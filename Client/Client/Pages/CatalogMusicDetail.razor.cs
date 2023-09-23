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

    private MusicCatalog? MusicCatalog { get; set; }
    private List<MusicCatalog>? CatalogMusics { get; set; }

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
}