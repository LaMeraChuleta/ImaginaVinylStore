using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualBasic.CompilerServices;
using SharedApp.Models;

namespace Client.App.Pages;

public partial class Index : ComponentBase
{
    [Inject] public HttpClient Http { get; set; }
    [Inject] public IHttpClientFactory HttpFactory { get; set; }
    private List<MusicCatalog> CatalogMusics { get; set; } = new();
    private List<Artist> Artists { get; set; } = new();

    public Index()
    {
    }

    protected override async Task OnInitializedAsync()
    {
        Http = HttpFactory.CreateClient("CatalogMusic.API");
        CatalogMusics = await Http.GetFromJsonAsync<List<MusicCatalog>>(nameof(MusicCatalog)) ??
                        throw new InvalidOperationException();

        Artists = await Http.GetFromJsonAsync<List<Artist>>(nameof(Artist)) ??
                  throw new IncompleteInitialization();
    }
}