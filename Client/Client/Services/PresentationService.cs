using Client.App.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using SharedApp.Models;

namespace Client.App.Services
{
    public class PresentationService : HttpClientHelperService, IPresentationService
    {
        public PresentationService(IHttpClientFactory httpClientFactory, IAccessTokenProvider tokenProvider)
            : base(httpClientFactory, tokenProvider)
        {
        }

        public Task<Presentation> CreateAsync(Presentation presentation)
        {
            return Post<Presentation>(nameof(Presentation), presentation);
        }

        public Task<List<Presentation>> GetAsync()
        {
            return Get<List<Presentation>>(nameof(Presentation));
        }
    }
}
