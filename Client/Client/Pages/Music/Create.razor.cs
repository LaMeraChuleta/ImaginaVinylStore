namespace Client.App.Pages.Music
{
    public partial class Create : ComponentBase
    {
        [Inject] public IToastService ToastService { get; set; }
        [Inject] public ICatalogMusicService CatalogMusicService { get; set; }
        [Inject] public IArtistService ArtistService { get; set; }
        [Inject] public IPresentationService PresentationService { get; set; }
        [Inject] public IGenreService GenreService { get; set; }
        [Inject] public IFormatService FormatService { get; set; }
        [Inject] public IProductService ProductService { get; set; }

        private MusicCatalog NewMusicCatalog { get; set; } = new();
        private List<Artist> Artists { get; set; } = new();
        private List<Genre> Genres { get; set; } = new();
        private List<Format> Formats { get; set; } = new();
        private List<Presentation> Presentations { get; set; } = new();
        private EditContext _editContextMusicCatalog;
        private bool IsLoading { get; set; }
        private bool IsAnyImage { get; set; }

        public delegate void CatalogMusicInsertOnDb(int idCatalogMusic);
        public static event CatalogMusicInsertOnDb OnCompleteInsert;

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            Artists = await ArtistService.GetAsync();
            Formats = await FormatService.GetAsync();
            Genres = await GenreService.GetAsync();
            Presentations = await PresentationService.GetAsync();
            _editContextMusicCatalog = new EditContext(NewMusicCatalog);
            IsLoading = false;
            StateHasChanged();
        }

        private async void CreateCatalogMusics()
        {
            try
            {
                if (!_editContextMusicCatalog.Validate()) return;

                NewMusicCatalog = await CatalogMusicService.CreateAsync(NewMusicCatalog);
                await ProductService.CreateCatalogOnStripeAsync(NewMusicCatalog!);
                OnCompleteInsert.Invoke(NewMusicCatalog.Id);
                ToastService.ShowToast(ToastLevel.Success, $"Exito se creo {NewMusicCatalog!.Title}-{NewMusicCatalog.Artist?.Name} en el catalogo");
                NewMusicCatalog = new MusicCatalog();
                StateHasChanged();
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