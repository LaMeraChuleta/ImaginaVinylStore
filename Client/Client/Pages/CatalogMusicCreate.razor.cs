using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.FileProviders;
using InvalidOperationException = System.InvalidOperationException;

namespace Client.App.Pages;

public partial class CatalogMusicCreate : ComponentBase
{
    [Inject] public HttpClient Http { get; set; }
    [Inject] public IHttpClientFactory HttpFactory { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    private MusicCatalog NewMusicCatalog { get; set; } = new();
    private List<IBrowserFile> PhotoMusicCatalog { get; set; } = new();
    private List<IBrowserFile> PhotoArtist { get; set; } = new();
    private List<string> PhotoMusicCatalogBase64 { get; set; } = new();
    private List<string> PhotoArtistsBase64 { get; set; } = new();
    
    private const long MaxFileSize = 1024 * 150 * 3;
    private const int MaxAllowedFiles = 3;

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

    public CatalogMusicCreate()
    {
    }

    protected override async Task OnInitializedAsync()
    {
        Http = HttpFactory.CreateClient("CatalogMusic.API");
        
        Artists = await Http.GetFromJsonAsync<List<Artist>>(nameof(Artist)) ??
                  throw new InvalidOperationException();
        Formats = await Http.GetFromJsonAsync<List<Format>>(nameof(Format)) ??
                  throw new InvalidOperationException();
        Genres = await Http.GetFromJsonAsync<List<Genre>>(nameof(Genre)) ?? 
                 throw new InvalidOperationException();
        Presentations = await Http.GetFromJsonAsync<List<Presentation>>(nameof(Presentation)) ??
                        throw new InvalidOperationException();
    }

    private async void CreateCatalogMusics()
    {
        var response = await Http.PostAsJsonAsync<MusicCatalog>(nameof(MusicCatalog), NewMusicCatalog);
        NewMusicCatalog = await response.Content.ReadFromJsonAsync<MusicCatalog>() ??
                          throw new InvalidOperationException();
        foreach (var file in PhotoMusicCatalog)
        {
            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream(MaxFileSize));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, nameof(file), file.Name);
            var result = await Http.PostAsync($"{nameof(MusicCatalog)}/Images?id={NewMusicCatalog?.Id}", content);
            var image = await result.Content.ReadFromJsonAsync<ImageCatalog>();
            NewMusicCatalog?.Images?.ToList().Add(image ?? throw new InvalidOperationException());
        }

        NewMusicCatalog = new MusicCatalog();
        PhotoMusicCatalog.Clear();
        StateHasChanged();
    }

    private async void CreateArtist()
    {
        var response = await Http.PostAsJsonAsync<Artist>(nameof(Artist), NewArtist);
        NewArtist = await response.Content.ReadFromJsonAsync<Artist>() ?? throw new InvalidOperationException();
        Artists.Add(NewArtist);
        
        foreach (var file in PhotoArtist)
        {
            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream(MaxFileSize));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, nameof(file), file.Name);
            var result = await Http.PostAsync($"{nameof(Artist)}/Images?id={NewArtist?.Id}", content);
            var image = await result.Content.ReadFromJsonAsync<ImageArtist>();
        }
        
        NewArtist = new Artist();
        ShowModalNewArtist = false;
        StateHasChanged();
    }

    private async void CreateGenre()
    {
        var response = await Http.PostAsJsonAsync<Genre>(nameof(Genre), NewGenre);
        Genres.Add(await response.Content.ReadFromJsonAsync<Genre>() ?? throw new InvalidOperationException());
        NewGenre = new Genre();
        ShowModalNewGenre = false;
        StateHasChanged();
    }

    private async void CreateFormat()
    {
        var response = await Http.PostAsJsonAsync<Format>(nameof(Format), NewFormat);
        Formats.Add(await response.Content.ReadFromJsonAsync<Format>() ?? throw new InvalidOperationException());
        NewFormat = new Format();
        ShowModalNewFormat = false;
        StateHasChanged();
    }

    private async void CreatePresentation()
    {
        var response = await Http.PostAsJsonAsync<Presentation>(nameof(Presentation), NewPresentation);
        Presentations.Add(await response.Content.ReadFromJsonAsync<Presentation>() ??
                          throw new InvalidOperationException());
        NewPresentation = new Presentation();
        ShowModalNewPresentation = false;
        StateHasChanged();
    }
    
    private async void SaveImageArtistNew(InputFileChangeEventArgs e)
    {
        foreach (var file in e.GetMultipleFiles(MaxAllowedFiles))
        {
            var buffer = new byte[file.Size];
            var readAsync = await file.OpenReadStream().ReadAsync(buffer);
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
            var readAsync = await file.OpenReadStream().ReadAsync(buffer);
            var imageDataUrl = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
            PhotoMusicCatalog.Add(file);
            PhotoMusicCatalogBase64.Add(imageDataUrl);
        }
        StateHasChanged();
    }
}