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
        public async Task<MusicCatalog> CreateAsync(MusicCatalog musicCatalog)
        {
            return await Post<MusicCatalog>(nameof(MusicCatalog), musicCatalog);
        }
        public async Task<ImageCatalog> CreateImageAsync(MusicCatalog musicCatalog, IBrowserFile file)
        {                                                         
            return await Post<ImageCatalog>($"{nameof(MusicCatalog)}/Images?id={musicCatalog?.Id}", ParseBrowserFile(file));
        }
        public Task<bool> UpdateAsync(MusicCatalog catalog)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteAsync(MusicCatalog catalog)
        {
            return await Delete<bool>(nameof(MusicCatalog), catalog.Id);
        }       
        private MultipartFormDataContent ParseBrowserFile(IBrowserFile file)
        {
            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream(MaxFileSize));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, nameof(file), file.Name);
            return content;
        }
    }
}
