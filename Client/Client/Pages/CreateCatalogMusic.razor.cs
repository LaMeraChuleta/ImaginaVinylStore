using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using SharedApp.Models;
using System.Net.Http.Json;

namespace Client.App.Pages
{
    public partial class CreateCatalogMusic : ComponentBase
    {
        [Inject] public HttpClient _Http { get; set; }
        [Inject] public IHttpClientFactory _HttpFactory { get; set; }
        [Inject] public NavigationManager _navigationManager { get; set; }
        
        private MusicCatalog NewMusicCatalog { get; set; } = new();
        private List<int> PhotoCatalogMusic { get; set; } = new();

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
            _Http = _HttpFactory.CreateClient("CatalogMusic.API");
            Presentations = await _Http.GetFromJsonAsync<List<Presentation>>("Presentation") ?? new();
            Formats = await _Http.GetFromJsonAsync<List<Format>>("Format");
            Artists = await _Http.GetFromJsonAsync<List<Artist>>("Artist");
            Genres = await _Http.GetFromJsonAsync<List<Genre>>("Genre");
        }

        private async void CreateCatalogMusics()
        {
            var response = await _Http.PostAsJsonAsync<MusicCatalog>("CatalogMusic", NewMusicCatalog);
            if (response != null) NewMusicCatalog = new();
            NewMusicCatalog = new();
            StateHasChanged();
        }
        private async void CreateArtist()
        {
            var response = await _Http.PostAsJsonAsync<Artist>("Artist", NewArtist);
            if (response != null) Artists.Add(await response.Content.ReadFromJsonAsync<Artist>());
            NewArtist = new();
            ShowModalNewArtist = false;
            StateHasChanged();
        }
        private async void CreateGenre()
        {
            var response = await _Http.PostAsJsonAsync<Genre>("Genre", NewGenre);
            if (response != null) Genres.Add(await response.Content.ReadFromJsonAsync<Genre>());
            NewGenre = new();
            ShowModalNewGenre = false;
            StateHasChanged();
        }
        private async void CreateFormat()
        {
            var response = await _Http.PostAsJsonAsync<Format>("Format", NewFormat);
            if (response != null) Formats.Add(await response.Content.ReadFromJsonAsync<Format>());
            _navigationManager.NavigateTo("/CreateCatalogMusic", forceLoad: true);
        }

        private async void CreatePresentation()
        {
            var response = await _Http.PostAsJsonAsync<Presentation>("Presentation", NewPresentation);
            if (response != null) Presentations.Add(await response.Content.ReadFromJsonAsync<Presentation>());
            NewPresentation = new();
            ShowModalNewPresentation = false;
            StateHasChanged();
        }
    }
}
