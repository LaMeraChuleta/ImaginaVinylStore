using Blazored.Toast.Services;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SharedApp.Models;

namespace Client.App.Pages
{
    public partial class CatalogMusicEdit : ComponentBase
    {
        [Parameter] public int MusicCatalogId { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IToastService ToastService { get; set; }
        [Inject] public ICatalogMusicService CatalogMusicService { get; set; }
        [Inject] public IArtistService ArtistService { get; set; }
        [Inject] public IPresentationService PresentationService { get; set; }
        [Inject] public IGenreService GenreService { get; set; }
        [Inject] public IFormatService FormatService { get; set; }
        private MusicCatalog EditMusicCatalog { get; set; } = new();
        private List<Artist> Artists { get; set; } = new();
        private List<Genre> Genres { get; set; } = new();
        private List<Format> Formats { get; set; } = new();
        private List<Presentation> Presentations { get; set; } = new();
        private EditContext _editContextMusicCatalog;        
        private bool IsLoading { get; set; }
        private bool IsAnyImage { get; set; }

        public delegate void CatalogMusicEditOnDb(int idCatalogMusic);
        public static event CatalogMusicEditOnDb OnCompleteEdit;

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            Artists = await ArtistService.GetAsync();
            Formats = await FormatService.GetAsync();
            Genres = await GenreService.GetAsync();
            Presentations = await PresentationService.GetAsync();
            EditMusicCatalog = await CatalogMusicService.GetByIdAsync(MusicCatalogId);
            _editContextMusicCatalog = new EditContext(EditMusicCatalog);
            IsLoading = false;
            StateHasChanged();
        }
        private async void EditCatalogMusics()
        {
            try
            {
                if (!_editContextMusicCatalog.Validate()) return;

                if (!await CatalogMusicService.UpdateAsync(EditMusicCatalog))
                {
                    ToastService.ShowToast(ToastLevel.Success, $"No se pudo actualizar {EditMusicCatalog!.Title}-{EditMusicCatalog.Artist?.Name} en el catalogo");
                }

                OnCompleteEdit.Invoke(EditMusicCatalog.Id);
                ToastService.ShowToast(ToastLevel.Success, $"Exito se actualizo {EditMusicCatalog!.Title}-{EditMusicCatalog.Artist?.Name} en el catalogo");
                StateHasChanged();
                NavigationManager.NavigateTo("/ManageCatalogMusic");
            }
            catch (Exception exception)
            {
                ToastService.ShowToast(ToastLevel.Error, exception.Message);
            }
        }
        public void SetIsAnyImageForCatalog(bool value)
        {
            IsAnyImage = value;
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
