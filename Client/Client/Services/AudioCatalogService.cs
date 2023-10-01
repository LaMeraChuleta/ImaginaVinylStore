using Client.App.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using SharedApp.Models;
using System.Net.Http.Headers;

namespace Client.App.Services
{
    public class AudioCatalogService : HttpClientHelperService, IAudioCatalogService
    {
        private const long MaxFileSize = 1024 * 150 * 3;
        public AudioCatalogService(IHttpClientFactory httpClientFactory, IAccessTokenProvider tokenProvider)
            : base(httpClientFactory, tokenProvider)
        {
        }
        public async Task<List<AudioCatalog>> GetAsync()
        {
            return await Get<List<AudioCatalog>>(nameof(AudioCatalog));
        }
        public async Task<AudioCatalog> CreateAsync(AudioCatalog audioCatalog)
        {
            return await Post(nameof(AudioCatalog), audioCatalog);
        }
        public async Task<ImageAudio> CreateImageAsync(AudioCatalog audioCatalog, IBrowserFile file)
        {
            return await Post<ImageAudio>($"{nameof(AudioCatalog)}/Images?id={audioCatalog?.Id}", ParseBrowserFile(file));
        }
        private MultipartFormDataContent ParseBrowserFile(IBrowserFile file)
        {
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream(MaxFileSize));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, nameof(file), file.Name);
            return content;
        }
    }
}
