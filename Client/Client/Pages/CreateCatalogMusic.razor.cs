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


        public CatalogMusic NewCatalogMusic { get; set; } = new();
        public List<int> PhotoCatalogMusic { get; set; } = new();

        public List<Artist> Artists { get; set; } = new();
        public Artist NewArtist { get; set; } = new();
        public bool ShowModalNewArtist { get; set; }

        public List<Genre> Genres { get; set; } = new();
        public Genre NewGenre { get; set; } = new();
        public bool ShowModalNewGenre { get; set; }

        public List<Format> Formats { get; set; } = new();
        public Format NewFormat { get; set; } = new();
        public bool ShowModalNewFormat { get; set; }

        public List<Presentation> Presentations { get; set; } = new();
        public Presentation NewPresentation { get; set; } = new();
        public bool ShowModalNewPresentation { get; set; }



        public CreateCatalogMusic()
        {
        }
        protected override async Task OnInitializedAsync()
        {
            _Http = _HttpFactory.CreateClient("CatalogMusic.API");
            Presentations = await _Http.GetFromJsonAsync<List<Presentation>>("Presentation");
            Formats = await _Http.GetFromJsonAsync<List<Format>>("Format");
            Artists = await _Http.GetFromJsonAsync<List<Artist>>("Artist");
            Genres = await _Http.GetFromJsonAsync<List<Genre>>("Genre");
        }

        private async void CreateCatalogMusics()
        {
            var response = await _Http.PostAsJsonAsync<CatalogMusic>("CatalogMusic", NewCatalogMusic);
            if (response != null) NewCatalogMusic = new();
            NewCatalogMusic = new();
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
