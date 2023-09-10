using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;

namespace Client.App.Shared;

public partial class SearchNavBarList : ComponentBase, IDisposable
{
    [Inject] public IHttpClientHelper HttpClientHelper { get; set; }
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
        var parameters = new Dictionary<string, string> { { "querySearch", query } };
        MusicCatalogs = await HttpClientHelper.Get<List<MusicCatalog>>($"{nameof(MusicCatalog)}/ForSearch", parameters);
        isSearching = false;
        StateHasChanged();
    }
}