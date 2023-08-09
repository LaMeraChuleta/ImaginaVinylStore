using Blazored.Toast.Services;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;

namespace Client.App.Pages;

public partial class CatalogMusicIndex : ComponentBase
{
    [Parameter] public string? TypeFormat { get; set; }
    [Inject] public IHttpClientHelper HttpClientHelper { get; set; }
    [Inject] public IToastService ToastService { get; set; }

    private List<Genre> Genres { get; set; } = new();
    private List<Format> Formats { get; set; } = new();
    private List<MusicCatalog> CatalogMusics { get; set; } = new();


    protected override async Task OnParametersSetAsync()
    {
        try
        {
            TypeFormat = TypeFormat;
            Genres = await HttpClientHelper.Get<List<Genre>>(nameof(Genre));
            Formats = await HttpClientHelper.Get<List<Format>>(nameof(Format));
            
            var idFormat = Formats.Find(x => x.Name == TypeFormat)!.Id.ToString();
            var parameter = new Dictionary<string, string>() { { "idFormat", idFormat } };

            CatalogMusics = await HttpClientHelper.Get<List<MusicCatalog>>($"{nameof(MusicCatalog)}/ForFilter", parameter);
        }
        catch (Exception ex)
        {
            ToastService.ShowToast(ToastLevel.Error, ex.Message);
        }
        await base.OnParametersSetAsync();
    }
}