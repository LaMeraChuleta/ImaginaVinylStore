using Blazored.Toast.Services;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;

namespace Client.App.Pages;

public partial class Index : ComponentBase
{
    private IEnumerable<MusicCatalog> CatalogMusics { get; set; } = Enumerable.Empty<MusicCatalog>();
    private IEnumerable<Artist> Artists { get; set; }
    [Inject] public IHttpClientHelperService HttpClientHelper { get; set; }
    [Inject] public IToastService ToastService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Artists = await HttpClientHelper.Get<List<Artist>>(nameof(Artist));
            CatalogMusics = await HttpClientHelper.Get<List<MusicCatalog>>(nameof(MusicCatalog));

            Artists = Artists.Take(10);
            CatalogMusics = CatalogMusics.OrderByDescending(x => x.Id).Take(10);
        }
        catch (Exception ex)
        {
            ToastService.ShowToast(ToastLevel.Error, ex.Message);
        }
    }
}