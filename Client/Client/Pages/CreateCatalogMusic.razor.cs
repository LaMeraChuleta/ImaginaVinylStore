using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text.Json;
using Client.App.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.FileProviders;
using InvalidOperationException = System.InvalidOperationException;

namespace Client.App.Pages;

public partial class CreateCatalogMusic : ComponentBase
{
    [Inject] public HttpClient Http { get; set; }
    [Inject] public IHttpClientFactory HttpFactory { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    public AlertMessage AlertMessage { get; set; } = new();
    
    
    private MusicCatalog NewMusicCatalog { get; set; } = new();
    private List<IBrowserFile> PhotoCatalogMusic { get; set; } = new();
    private List<string> PhotoCatalogMusicBase64 { get; set; } = new();
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

    public CreateCatalogMusic()
    {
    }

    protected override async Task OnInitializedAsync()
    {
        Http = HttpFactory.CreateClient("CatalogMusic.API");
        Presentations = await Http.GetFromJsonAsync<List<Presentation>>(nameof(Presentation)) ??
                        throw new InvalidOperationException();
        Artists = await Http.GetFromJsonAsync<List<Artist>>(nameof(Artist)) ??
                  throw new InvalidOperationException();
        Formats = await Http.GetFromJsonAsync<List<Format>>(nameof(Format)) ??
                  throw new InvalidOperationException();
        Genres = await Http.GetFromJsonAsync<List<Genre>>(nameof(Genre)) ?? throw new InvalidOperationException();
    }

    private async void CreateCatalogMusics()
    {
        try
        {
            var response = await Http.PostAsJsonAsync<MusicCatalog>(nameof(MusicCatalog), NewMusicCatalog);
            NewMusicCatalog = await response.Content.ReadFromJsonAsync<MusicCatalog>() ?? new MusicCatalog();

            foreach (var file in PhotoCatalogMusic)
            {
                using var content = new MultipartFormDataContent();
                var fileContent = new StreamContent(file.OpenReadStream(MaxFileSize));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, nameof(file), file.Name);
                var result = await Http.PostAsync($"{nameof(MusicCatalog)}/Images?id={NewMusicCatalog?.Id}", content);
                var image = await result.Content.ReadFromJsonAsync<ImageCatalog>();
                NewMusicCatalog?.Images?.ToList().Add(image ?? new ImageCatalog());
            }

            AlertMessage.Show("Éxito", $"Se creó el nuevo {NewMusicCatalog?.Format?.Name} de {NewMusicCatalog?.Artist?.Name} - {NewMusicCatalog?.Title}", AlertMessage.TypeAlert.Success);
            NewMusicCatalog = new MusicCatalog();
            PhotoCatalogMusic.Clear();
            StateHasChanged();
        }
        catch (HttpRequestException ex)
        {
            AlertMessage.Show("Error de solicitud", ex.Message, AlertMessage.TypeAlert.Error);
        }
        catch (JsonException ex)
        {
            AlertMessage.Show("Error de deserialización", ex.Message, AlertMessage.TypeAlert.Error);
        }
        catch (Exception ex)
        {
            AlertMessage.Show("Error", ex.Message, AlertMessage.TypeAlert.Error);
        }
    }

    private async void CreateArtist()
    {
        try
        {
            var response = await Http.PostAsJsonAsync<Artist>(nameof(Artist), NewArtist);
            Artists.Add(await response.Content.ReadFromJsonAsync<Artist>() ?? throw new InvalidOperationException());
            ShowModalNewArtist = false;
            AlertMessage.Show("Exito", $"Se creo el nuevo artista {Artists.LastOrDefault()?.Name}", AlertMessage.TypeAlert.Success);
            NewArtist = new Artist();
            StateHasChanged();
        }
        catch (HttpRequestException ex)
        {
            AlertMessage.Show("Error de solicitud", ex.Message, AlertMessage.TypeAlert.Error);
        }
        catch (JsonException ex)
        {
            AlertMessage.Show("Error de deserialización", ex.Message, AlertMessage.TypeAlert.Error);
        }
        catch (Exception ex)
        {
            AlertMessage.Show("Error", ex.Message, AlertMessage.TypeAlert.Error);
        }
    }

    private async void CreateGenre()
    {
        var response = await Http.PostAsJsonAsync<Genre>(nameof(Genre), NewGenre);
        Genres.Add(await response.Content.ReadFromJsonAsync<Genre>() ?? throw new InvalidOperationException());
        NewGenre = new Genre();
        ShowModalNewGenre = false;
        StateHasChanged();
    }

    private async void Test()
    {
        AlertMessage.Show("Exito", "test", AlertMessage.TypeAlert.Success);
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

    private async void SaveImageNew(InputFileChangeEventArgs e)
    {
        foreach (var file in e.GetMultipleFiles(MaxAllowedFiles))
        {
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);
            var imageDataUrl = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
            PhotoCatalogMusic.Add(file);
            PhotoCatalogMusicBase64.Add(imageDataUrl);
        }
        StateHasChanged();
    }
}