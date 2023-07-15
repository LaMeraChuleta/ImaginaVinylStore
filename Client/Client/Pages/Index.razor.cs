using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.VisualBasic.CompilerServices;
using SharedApp.Models;

namespace Client.App.Pages;

public partial class Index : ComponentBase
{
    [Inject] public HttpClient Http { get; set; }
    [Inject] public IHttpClientFactory HttpFactory { get; set; }
    [Inject] public IAccessTokenProvider TokenProvider { get; set; }
    private List<MusicCatalog> CatalogMusics { get; set; } = new();
    private List<Artist> Artists { get; set; } = new();

    public Index()
    {
    }

    protected override async Task OnInitializedAsync()
    {
        var accessTokenResult = await TokenProvider.RequestAccessToken();
        var token = string.Empty;

        if (accessTokenResult.TryGetToken(out var accessToken))
        {
            token = accessToken.Value;
        }
        
        Http = HttpFactory.CreateClient("CatalogMusic.API");
        Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        CatalogMusics = await Http.GetFromJsonAsync<List<MusicCatalog>>(nameof(MusicCatalog)) ??
                        throw new InvalidOperationException();

        Artists = await Http.GetFromJsonAsync<List<Artist>>(nameof(Artist)) ??
                  throw new IncompleteInitialization();
    }
}