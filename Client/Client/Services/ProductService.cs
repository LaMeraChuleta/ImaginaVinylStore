using Client.App.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using SharedApp.Models;

namespace Client.App.Services
{
    public class ProductService : HttpClientHelperService, IProductService
    {

        public ProductService(IHttpClientFactory httpClientFactory, IAccessTokenProvider tokenProvider)
            : base(httpClientFactory, tokenProvider)
        {
        }
        public Task<MusicCatalog> CreateAsync(MusicCatalog musicCatalog)
        {
            return Post<MusicCatalog>("Product/MusicCatalog", musicCatalog);
        }

        public Task<AudioCatalog> CreateAsync(AudioCatalog audioCatalog)
        {
            return Post<AudioCatalog>("Product/AudioCatalog", audioCatalog);
        }
    }
}
