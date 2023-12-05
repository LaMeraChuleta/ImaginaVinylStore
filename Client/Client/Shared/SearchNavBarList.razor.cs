using static Client.App.Services.CatalogMusicService;

namespace Client.App.Shared
{
    public partial class SearchNavBarList : ComponentBase, IDisposable
    {
        [Inject] public ICatalogMusicService CatalogMusicService { get; set; }
        private List<MusicCatalog> MusicCatalogs { get; set; } = new();
        private bool isSearching { get; set; }

        public void Dispose()
        {
            NavBar.OnSearchCatalog -= HandleSearchCatalog;
            base.OnInitialized();
        }

        protected override void OnInitialized()
        {
            NavBar.OnSearchCatalog += HandleSearchCatalog;
            base.OnInitialized();
        }

        private async void HandleSearchCatalog(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                MusicCatalogs.Clear();
                StateHasChanged();
                return;
            }

            isSearching = true;
            StateHasChanged();
            MusicCatalogs = await CatalogMusicService.GetAsync(new FilterForCatalogMusic() { QuerySearch = query });
            isSearching = false;
            StateHasChanged();
        }
    }
}