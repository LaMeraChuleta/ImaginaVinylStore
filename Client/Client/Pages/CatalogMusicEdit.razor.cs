using Blazored.Toast.Services;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SharedApp.Models;
using static Client.App.Services.CatalogMusicService;

namespace Client.App.Pages
{
    public partial class CatalogMusicEdit : ComponentBase
    {
        [Parameter] public int MusicCatalogId { get; set; }
        [Inject] public IToastService ToastService { get; set; }
        [Inject] public ICatalogMusicService CatalogMusicService { get; set; }
        [Inject] public IArtistService ArtistService { get; set; }
        [Inject] public IPresentationService PresentationService { get; set; }
        [Inject] public IGenreService GenreService { get; set; }
        [Inject] public IFormatService FormatService { get; set; }
        [Inject] public IProductService ProductService { get; set; }

        private MusicCatalog NewMusicCatalog { get; set; } = new();
        private List<IBrowserFile> PhotoMusicCatalog { get; } = new();
        private List<string> PhotoMusicCatalogBase64 { get; } = new();
        private List<Artist> Artists { get; set; } = new();
        private List<Genre> Genres { get; set; } = new();
        private List<Format> Formats { get; set; } = new();
        private List<Presentation> Presentations { get; set; } = new();
        private EditContext _editContextMusicCatalog;
        private const int MaxAllowedFiles = 3;
        private bool IsLoading { get; set; }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            Artists = await ArtistService.GetAsync();
            Formats = await FormatService.GetAsync();
            Genres = await GenreService.GetAsync();
            Presentations = await PresentationService.GetAsync();
            NewMusicCatalog = await CatalogMusicService.GetByIdAsync(new FilterForCatalogMusic() { Id = MusicCatalogId.ToString() });
            _editContextMusicCatalog = new EditContext(NewMusicCatalog);
            IsLoading = false;
            StateHasChanged();
        }
        private async void EditCatalogMusics()
        {
            try
            {
                if (!_editContextMusicCatalog.Validate()) return;

                await CatalogMusicService.UpdateAsync(NewMusicCatalog);
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
        private async void SaveImage(InputFileChangeEventArgs e, List<IBrowserFile> photos, List<string> photoBase64)
        {
            foreach (var file in e.GetMultipleFiles(MaxAllowedFiles))
            {
                var buffer = new byte[file.Size];
                var _ = await file.OpenReadStream().ReadAsync(buffer);
                var imageDataUrl = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
                photos.Add(file);
                photoBase64.Add(imageDataUrl);
            }

            StateHasChanged();
        }
        private void NewCatalogForMusic((string, object) value)
        {
            switch (value.Item1)
            {
                case nameof(Artist):
                    Artists.Add((Artist)value.Item2);
                    break;
                case nameof(Genre):
                    Genres.Add((Genre)value.Item2);
                    break;
                case nameof(Format):
                    Formats.Add((Format)value.Item2);
                    break;
                case nameof(Presentation):
                    Presentations.Add((Presentation)value.Item2);
                    break;
            }
            StateHasChanged();
        }
    }
}
