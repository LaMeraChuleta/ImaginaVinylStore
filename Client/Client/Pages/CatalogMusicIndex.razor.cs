using Blazored.Toast.Services;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;
using static Client.App.Services.CatalogMusicService;

namespace Client.App.Pages;

public partial class CatalogMusicIndex : ComponentBase
{
    [Parameter] public string TypeFormat { get; set; }
    [Inject] public ICatalogMusicService CatalogMusicService { get; set; }
    [Inject] public IArtistService ArtistService { get; set; }
    [Inject] public IGenreService GenreService { get; set; }
    [Inject] public IFormatService FormatService { get; set; }
    [Inject] public IPresentationService PresentationService { get; set; }
    [Inject] public IToastService ToastService { get; set; }

    private List<Artist> Artists { get; set; } = new();
    private List<Genre> Genres { get; set; } = new();
    private List<Format> Formats { get; set; } = new();
    private List<Presentation> Presentations { get; set; } = new();
    private List<MusicCatalog> CatalogMusics { get; set; } = new();
    private FilterForCatalogMusic Filter { get; } = new();


    protected override async Task OnParametersSetAsync()
    {
        try
        {
            Artists = await ArtistService.GetAsync();
            Genres = await GenreService.GetAsync();
            Presentations = await PresentationService.GetAsync();
            Presentations = Presentations.Where(x => x.FormatId == Filter.IdFormat).ToList();
            Formats = await FormatService.GetAsync();

            Filter.IdFormat = Formats.Find(x => x.Name == TypeFormat)!.Id;
            CatalogMusics = await CatalogMusicService.GetAsync(Filter);
        }
        catch (Exception ex)
        {
            ToastService.ShowToast(ToastLevel.Error, ex.Message);
        }

        await base.OnParametersSetAsync();
    }

    private async Task FilterCatalogMusic()
    {
        try
        {
            CatalogMusics = await CatalogMusicService.GetAsync(Filter);
            StateHasChanged();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}