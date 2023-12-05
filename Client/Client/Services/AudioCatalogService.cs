

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
        public Task<AudioCatalog> GetByIdAsync(int id)
        {
            return Get<AudioCatalog>(nameof(AudioCatalog), id);
        }
        public async Task<AudioCatalog> CreateAsync(AudioCatalog audioCatalog)
        {
            return await Post(nameof(AudioCatalog), audioCatalog);
        }
        public async Task<ImageAudio> CreateImageAsync(int idAudioCatalog, IBrowserFile file)
        {
            return await Post<ImageAudio>($"{nameof(AudioCatalog)}/Images?id={idAudioCatalog}", ParseBrowserFile(file));
        }
        public async Task<bool> UpdateAsync(AudioCatalog audioCatalog)
        {
            return await Put<bool, AudioCatalog>(nameof(AudioCatalog), audioCatalog.Id, audioCatalog);
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
