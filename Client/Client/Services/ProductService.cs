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
        public Task<MusicCatalog> CreateCatalogOnStripeAsync(MusicCatalog musicCatalog)
        {
            return Post("Product/MusicCatalog", musicCatalog);
        }

        public Task<AudioCatalog> CreateCatalogOnStripeAsync(AudioCatalog audioCatalog)
        {
            return Post("Product/AudioCatalog", audioCatalog);
        }
    }
}
