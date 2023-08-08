using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;

namespace Client.App.Pages;

public partial class Index : ComponentBase
{
    private List<MusicCatalog> CatalogMusics { get; set; } = new();
    private List<Artist> Artists { get; set; } = new();
    [Inject] public IHttpClientHelper HttpClientHelper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Artists = await HttpClientHelper.Get<Artist>(nameof(Artist));
        CatalogMusics = await HttpClientHelper.Get<MusicCatalog>(nameof(MusicCatalog));
    }
}