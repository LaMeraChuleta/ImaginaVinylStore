using Client.App.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using SharedApp.Models;

namespace Client.App.Services
{
    public class OrderService : HttpClientHelperService, IOrderService
    {
        public OrderService(IHttpClientFactory httpClientFactory, IAccessTokenProvider tokenProvider)
            : base(httpClientFactory, tokenProvider)
        {
        }

        public async Task<List<Orders>> GetAsync()
        {
            return await Get<List<Orders>>(nameof(Orders));
        }
    }
}
