namespace Client.App.Services
{
    public class GenreService : HttpClientHelperService, IGenreService
    {
        public GenreService(IHttpClientFactory httpClientFactory, IAccessTokenProvider tokenProvider)
            : base(httpClientFactory, tokenProvider)
        {
        }

        public Task<Genre> CreateAsync(Genre genre)
        {
            return Post<Genre>(nameof(Genre), genre);
        }
        public Task<List<Genre>> GetAsync()
        {
            return Get<List<Genre>>(nameof(Genre));
        }
    }
}