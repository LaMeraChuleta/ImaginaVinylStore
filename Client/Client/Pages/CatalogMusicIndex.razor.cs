using Blazored.Toast.Services;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;

namespace Client.App.Pages;

public partial class CatalogMusicIndex : ComponentBase
{
    [Parameter] public string TypeFormat { get; set; }
    [Inject] public IHttpClientHelperService HttpClientHelper { get; set; }
    [Inject] public IToastService ToastService { get; set; }

    private List<Artist> Artists { get; set; } = new();
    private List<Genre> Genres { get; set; } = new();
    private List<Format> Formats { get; set; } = new();
    private List<Presentation> Presentations { get; set; } = new();
    private List<MusicCatalog> CatalogMusics { get; set; } = new();
    private FilterForCatalogMusic Filter { get; } = new();


    protected override async Task OnParametersSetAsync()
    {
        try
        {
            Genres = await HttpClientHelper.Get<List<Genre>>(nameof(Genre));
            Presentations = await HttpClientHelper.Get<List<Presentation>>(nameof(Presentation));
            Formats = await HttpClientHelper.Get<List<Format>>(nameof(Format));
            Artists = await HttpClientHelper.Get<List<Artist>>(nameof(Artist));

            Filter.IdFormat = Formats.Find(x => x.Name == TypeFormat)!.Id;
            var parameter = Filter.ParseToDictionary();
            CatalogMusics =
                await HttpClientHelper.Get<List<MusicCatalog>>($"{nameof(MusicCatalog)}/ForFilter", parameter);
            Presentations = Presentations.Where(x => x.FormatId == Filter.IdFormat).ToList();
        }
        catch (Exception ex)
        {
            ToastService.ShowToast(ToastLevel.Error, ex.Message);
        }

        await base.OnParametersSetAsync();
    }

    private async Task FilterCatalogMusic()
    {
        try
        {
            var parameter = Filter.ParseToDictionary();
            CatalogMusics =
                await HttpClientHelper.Get<List<MusicCatalog>>($"{nameof(MusicCatalog)}/ForFilter", parameter);
            StateHasChanged();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    internal class FilterForCatalogMusic
    {
        internal string Title { get; set; }
        internal int IdArtist { get; set; }
        internal int IdGenre { get; set; }
        internal int IdFormat { get; set; }
        internal int IdPresentation { get; set; }

        internal Dictionary<string, string> ParseToDictionary()
        {
            var parameter = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(Title)) parameter.Add("title", Title);
            if (IdArtist != 0) parameter.Add("idArtist", IdArtist.ToString());
            if (IdGenre != 0) parameter.Add("idGenre", IdGenre.ToString());
            if (IdFormat != 0) parameter.Add("idFormat", IdFormat.ToString());
            if (IdPresentation != 0) parameter.Add("idPresentation", IdPresentation.ToString());

            return parameter;
        }
    }
}