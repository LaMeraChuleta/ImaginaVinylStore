using Blazored.Toast.Services;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;

namespace Client.App.Pages;

public partial class Index : ComponentBase
{
    private IEnumerable<MusicCatalog> CatalogMusics { get; set; } = Enumerable.Empty<MusicCatalog>();
    private IEnumerable<AudioCatalog> AudioCatalogs { get; set; } = Enumerable.Empty<AudioCatalog>();
    private IEnumerable<Artist> Artists { get; set; } = Enumerable.Empty<Artist>();
    [Inject] public IToastService ToastService { get; set; }
    [Inject] public ICatalogMusicService CatalogMusicService { get; set; }
    [Inject] public IAudioCatalogService AudioCatalogService { get; set; }
    [Inject] public IArtistService ArtistService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Artists = (await ArtistService.GetAsync()).Take(10);
            CatalogMusics = (await CatalogMusicService.GetAsync())
                .OrderByDescending(x => x.Id)
                .Take(10);

            AudioCatalogs = await AudioCatalogService.GetAsync();
        }
        catch (Exception ex)
        {
            ToastService.ShowToast(ToastLevel.Error, ex.Message);
        }
    }
}