using Blazored.Toast.Services;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SharedApp.Models;

namespace Client.App.Pages;

public partial class CatalogMusicCreate : ComponentBase
{
    private const long MaxFileSize = 1024 * 150 * 3;
    private const int MaxAllowedFiles = 3;
    private EditContext _editContextArtist;
    private EditContext _editContextFormat;
    private EditContext _editContextGenre;
    private EditContext _editContextMusicCatalog;
    private EditContext _editContextPresentation;

    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IToastService ToastService { get; set; }
    [Inject] public IHttpClientHelperService HttpClientHelper { get; set; }
    [Inject] public ICatalogMusicService CatalogMusicService { get; set; }

    private MusicCatalog NewMusicCatalog { get; set; } = new();
    private List<IBrowserFile> PhotoMusicCatalog { get; } = new();
    private List<IBrowserFile> PhotoArtist { get; } = new();
    private List<string> PhotoMusicCatalogBase64 { get; } = new();
    private List<string> PhotoArtistsBase64 { get; } = new();

    [Inject] public IArtistService ArtistService { get; set; }
    private List<Artist> Artists { get; set; } = new();
    private Artist NewArtist { get; set; } = new();
    private bool ShowModalNewArtist { get; set; }

    private List<Genre> Genres { get; set; } = new();
    private Genre NewGenre { get; set; } = new();
    private bool ShowModalNewGenre { get; set; }

    private List<Format> Formats { get; set; } = new();
    private Format NewFormat { get; set; } = new();
    private bool ShowModalNewFormat { get; set; }

    private List<Presentation> Presentations { get; set; } = new();
    private Presentation NewPresentation { get; set; } = new();
    private bool ShowModalNewPresentation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _editContextArtist = new EditContext(NewArtist);
        Artists = await ArtistService.GetAsync();

        _editContextFormat = new EditContext(NewFormat);
        Formats = await HttpClientHelper.Get<List<Format>>(nameof(Format));

        _editContextGenre = new EditContext(NewGenre);
        Genres = await HttpClientHelper.Get<List<Genre>>(nameof(Genre));

        _editContextPresentation = new EditContext(NewPresentation);
        Presentations = await HttpClientHelper.Get<List<Presentation>>(nameof(Presentation));

        _editContextMusicCatalog = new EditContext(NewMusicCatalog);
    }

    private async void CreateCatalogMusics()
    {
        try
        {
            if (!_editContextMusicCatalog.Validate()) return;
            
            NewMusicCatalog = await CatalogMusicService.CreateAsync(NewMusicCatalog);
            foreach (var file in PhotoMusicCatalog)
            {
                var image = await CatalogMusicService.CreateImageAsync(NewMusicCatalog!, file);
                NewMusicCatalog?.Images?.ToList().Add(image);
            }

            PhotoMusicCatalog.Clear();
            NewMusicCatalog = new MusicCatalog();

            ToastService.ShowToast(ToastLevel.Success, $"Exito se creo {NewMusicCatalog!.Title}-{NewMusicCatalog.Artist?.Name} en el catalogo");                        
            StateHasChanged();
        }
        catch (Exception exception)
        {
            ToastService.ShowToast(ToastLevel.Error, exception.Message);
        }
    }

    private async void CreateArtist()
    {
        try
        {
            if (!_editContextArtist.Validate()) return;

            NewArtist = await ArtistService.CreateAsync(NewArtist);
            Artists.Add(NewArtist);

            foreach (var file in PhotoArtist)
            {
                await ArtistService.CreateImageAsync(NewArtist, file);
            }

            ToastService.ShowToast(ToastLevel.Success, $"Exito se creo el artista {NewArtist?.Name}");
            NewArtist = new Artist();
            ShowModalNewArtist = false;
            StateHasChanged();
        }
        catch (Exception exception)
        {
            ToastService.ShowToast(ToastLevel.Error, exception.Message);
        }
    }

    private async void CreateGenre()
    {
        try
        {
            if (!_editContextGenre.Validate()) return;

            NewGenre = await HttpClientHelper.Post(nameof(Genre), NewGenre);
            Genres.Add(NewGenre);
            ToastService.ShowToast(ToastLevel.Success, $"Exito se creo el genero {NewGenre.Name}");
            NewGenre = new Genre();
            ShowModalNewGenre = false;
            StateHasChanged();
        }
        catch (Exception exception)
        {
            ToastService.ShowToast(ToastLevel.Error, exception.Message);
        }
    }

    private async void CreateFormat()
    {
        try
        {
            if (!_editContextFormat.Validate()) return;

            NewFormat = await HttpClientHelper.Post(nameof(Format), NewFormat);
            Formats.Add(NewFormat);
            ToastService.ShowToast(ToastLevel.Success, $"Exito se creo el formato {NewFormat.Name}");
            NewFormat = new Format();
            ShowModalNewFormat = false;
            StateHasChanged();
        }
        catch (Exception exception)
        {
            ToastService.ShowToast(ToastLevel.Error, exception.Message);
        }
    }

    private async void CreatePresentation()
    {
        try
        {
            if (!_editContextPresentation.Validate()) return;

            NewPresentation = await HttpClientHelper.Post(nameof(Presentation), NewPresentation);
            Presentations.Add(NewPresentation);
            ToastService.ShowToast(ToastLevel.Success, $"Exito se creo la presentacion {NewPresentation.Name}");
            NewPresentation = new Presentation();
            ShowModalNewPresentation = false;
            StateHasChanged();
        }
        catch (Exception exception)
        {
            ToastService.ShowToast(ToastLevel.Error, exception.Message);
        }
    }

    private async void SaveImageArtistNew(InputFileChangeEventArgs e)
    {
        foreach (var file in e.GetMultipleFiles(MaxAllowedFiles))
        {
            var buffer = new byte[file.Size];
            var _ = await file.OpenReadStream().ReadAsync(buffer);
            var imageDataUrl = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
            PhotoArtist.Add(file);
            PhotoArtistsBase64.Add(imageDataUrl);
        }

        StateHasChanged();
    }

    private async void SaveImageMusicCatalogNew(InputFileChangeEventArgs e)
    {
        foreach (var file in e.GetMultipleFiles(MaxAllowedFiles))
        {
            var buffer = new byte[file.Size];
            var _ = await file.OpenReadStream().ReadAsync(buffer);
            var imageDataUrl = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
            PhotoMusicCatalog.Add(file);
            PhotoMusicCatalogBase64.Add(imageDataUrl);
        }

        StateHasChanged();
    }
}