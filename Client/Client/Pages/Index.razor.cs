using Blazored.Toast.Services;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;

namespace Client.App.Pages;

public partial class Index : ComponentBase
{
    private List<MusicCatalog> CatalogMusics { get; set; } = new();
    private List<Artist> Artists { get; set; } = new();
    [Inject] public IHttpClientHelper HttpClientHelper { get; set; }
    [Inject] public IToastService ToastService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Artists = await HttpClientHelper.Get<List<Artist>>(nameof(Artist));
            CatalogMusics = await HttpClientHelper.Get<List<MusicCatalog>>(nameof(MusicCatalog));
        }
        catch (Exception ex)
        {
            ToastService.ShowToast(ToastLevel.Error, ex.Message);
        }
    }
}