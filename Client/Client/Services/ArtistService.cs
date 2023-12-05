namespace Client.App.Services
{
    public class ArtistService : HttpClientHelperService, IArtistService
    {
        private const long MaxFileSize = 1024 * 150 * 3;
        public ArtistService(IHttpClientFactory httpClientFactory, IAccessTokenProvider tokenProvider)
            : base(httpClientFactory, tokenProvider)
        {
        }
        public async Task<List<Artist>> GetAsync()
        {
            return await Get<List<Artist>>(nameof(Artist));
        }
        public async Task<Artist> CreateAsync(Artist artist)
        {
            return await Post<Artist>(nameof(Artist), artist);
        }
        public async Task<ImageArtist> CreateImageAsync(Artist artist, IBrowserFile file)
        {
            return await Post<ImageArtist>($"{nameof(Artist)}/Images?id={artist?.Id}", ParseBrowserFile(file));
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
