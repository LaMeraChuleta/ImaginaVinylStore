using Blazored.Toast.Services;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SharedApp.Models;

namespace Client.App.Components
{
    public partial class CatalogsForMusic : ComponentBase
    {
        [Parameter] public EventCallback<(string, object)> SendNewCatalogForMusic { get; set; }
        [Inject] public IToastService ToastService { get; set; }
        [Inject] public IArtistService ArtistService { get; set; }
        [Inject] public IGenreService GenreService { get; set; }
        [Inject] public IFormatService FormatService { get; set; }
        [Inject] public IPresentationService PresentationService { get; set; }
        private List<IBrowserFile> PhotoArtist { get; } = new();
        private List<string> PhotoArtistsBase64 { get; } = new();
        private Artist NewArtist { get; set; } = new();
        private bool ShowModalNewArtist { get; set; }
        private Genre NewGenre { get; set; } = new();
        private bool ShowModalNewGenre { get; set; }
        private List<Format> Formats { get; set; } = new();
        private Format NewFormat { get; set; } = new();
        private bool ShowModalNewFormat { get; set; }
        private Presentation NewPresentation { get; set; } = new();
        private bool ShowModalNewPresentation { get; set; }
        private const int MaxAllowedFiles = 3;
        private EditContext _editContextArtist;
        private EditContext _editContextFormat;
        private EditContext _editContextGenre;
        private EditContext _editContextPresentation;

        protected override async Task OnInitializedAsync()
        {

            Formats = await FormatService.GetAsync();
            _editContextArtist = new EditContext(NewArtist);
            _editContextFormat = new EditContext(NewFormat);
            _editContextGenre = new EditContext(NewGenre);
            _editContextPresentation = new EditContext(NewPresentation);
            StateHasChanged();
        }

        private async void CreateArtist()
        {
            try
            {
                if (!_editContextArtist.Validate()) return;

                NewArtist = await ArtistService.CreateAsync(NewArtist);
                foreach (var file in PhotoArtist)
                {
                    await ArtistService.CreateImageAsync(NewArtist, file);
                }

                await SendNewCatalogForMusic.InvokeAsync((nameof(Artist), NewArtist));
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

                NewGenre = await GenreService.CreateAsync(NewGenre);

                await SendNewCatalogForMusic.InvokeAsync((nameof(Genre), NewGenre));
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

                NewFormat = await FormatService.CreateAsync(NewFormat);

                await SendNewCatalogForMusic.InvokeAsync((nameof(Format), NewFormat));
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

                NewPresentation = await PresentationService.CreateAsync(NewPresentation);

                await SendNewCatalogForMusic.InvokeAsync((nameof(Presentation), NewPresentation));
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
    }
}
