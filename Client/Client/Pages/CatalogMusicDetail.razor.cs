using Blazored.Toast.Services;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;

namespace Client.App.Pages;

public partial class CatalogMusicDetail : ComponentBase
{
    [Parameter] public int IdMusicCatalog { get; set; }
    [Inject] public IHttpClientHelper HttpClientHelper { get; set; }
    [Inject] public IToastService ToastService { get; set; }

    private MusicCatalog? MusicCatalog { get; set; }
    private List<MusicCatalog>? CatalogMusics { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            CatalogMusics = await HttpClientHelper.Get<List<MusicCatalog>>(nameof(MusicCatalog));

            var parameters = new Dictionary<string, string>() { { "id", IdMusicCatalog.ToString() } };
            MusicCatalog = await HttpClientHelper.Get<MusicCatalog>($"{nameof(MusicCatalog)}/ById", parameters);
        }
        catch (Exception ex)
        {
            ToastService.ShowToast(ToastLevel.Error, ex.Message);
        }
        await base.OnInitializedAsync();
    }
}