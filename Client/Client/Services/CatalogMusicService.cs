using Client.App.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using SharedApp.Models;
using System.Net.Http.Headers;

namespace Client.App.Services
{
    public class CatalogMusicService : HttpClientHelperService, ICatalogMusicService
    {
        private const long MaxFileSize = 1024 * 150 * 3;
        public CatalogMusicService(IHttpClientFactory httpClientFactory, IAccessTokenProvider tokenProvider)
            : base(httpClientFactory, tokenProvider)
        {
        }
        public async Task<List<MusicCatalog>> GetAsync()
        {
            return await Get<List<MusicCatalog>>(nameof(MusicCatalog));
        }
        public Task<MusicCatalog> GetByIdAsync(FilterForCatalogMusic filter)
        {
            return Get<MusicCatalog>($"{nameof(MusicCatalog)}/ById", filter.ParseToDictionary());
        }
        public Task<List<MusicCatalog>> GetAsync(FilterForCatalogMusic filter)
        {
            return Get<List<MusicCatalog>>($"{nameof(MusicCatalog)}/ForFilter", filter.ParseToDictionary());
        }
        public async Task<MusicCatalog> CreateAsync(MusicCatalog musicCatalog)
        {
            return await Post(nameof(MusicCatalog), musicCatalog);
        }
        public async Task<ImageCatalog> CreateImageAsync(MusicCatalog musicCatalog, IBrowserFile file)
        {
            return await Post<ImageCatalog>($"{nameof(MusicCatalog)}/Images?id={musicCatalog?.Id}", ParseBrowserFile(file));
        }
        public async Task<bool> UpdateAsync(MusicCatalog musicCatalog)
        {
            return await Put<bool, MusicCatalog>(nameof(MusicCatalog), musicCatalog.Id, musicCatalog);
        }
        public async Task<bool> DeleteAsync(MusicCatalog musicCatalog)
        {
            return await Delete<bool>(nameof(MusicCatalog), musicCatalog.Id);
        }
        private MultipartFormDataContent ParseBrowserFile(IBrowserFile file)
        {
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream(MaxFileSize));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, nameof(file), file.Name);
            return content;
        }

        public class FilterForCatalogMusic
        {
            internal string Id { get; set; }
            internal string Title { get; set; }
            internal string QuerySearch { get; set; }
            internal int IdArtist { get; set; }
            internal int IdGenre { get; set; }
            internal int IdFormat { get; set; }
            internal int IdPresentation { get; set; }

            internal Dictionary<string, string> ParseToDictionary()
            {
                var parameter = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(Id)) parameter.Add("id", Id);
                if (!string.IsNullOrEmpty(Title)) parameter.Add("title", Title);
                if (!string.IsNullOrEmpty(QuerySearch)) parameter.Add("querySearch", QuerySearch);
                if (IdArtist != 0) parameter.Add("idArtist", IdArtist.ToString());
                if (IdGenre != 0) parameter.Add("idGenre", IdGenre.ToString());
                if (IdFormat != 0) parameter.Add("idFormat", IdFormat.ToString());
                if (IdPresentation != 0) parameter.Add("idPresentation", IdPresentation.ToString());

                return parameter;
            }
        }
    }
}
