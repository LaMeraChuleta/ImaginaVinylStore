using Client.App.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using SharedApp.Models;

namespace Client.App.Services
{
    public class FormatService : HttpClientHelperService, IFormatService
    {
        public FormatService(IHttpClientFactory httpClientFactory, IAccessTokenProvider tokenProvider)
            : base(httpClientFactory, tokenProvider)
        {
        }

        public Task<Format> CreateAsync(Format format)
        {
            return Post<Format>(nameof(Format), format);
        }

        public Task<List<Format>> GetAsync()
        {
            return Get<List<Format>>(nameof(Format));
        }
    }
}
