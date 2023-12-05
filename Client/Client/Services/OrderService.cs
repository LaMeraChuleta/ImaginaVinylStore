﻿namespace Client.App.Services
{
    public class OrderService : HttpClientHelperService, IOrderService
    {
        public OrderService(IHttpClientFactory httpClientFactory, IAccessTokenProvider tokenProvider)
            : base(httpClientFactory, tokenProvider)
        {
        }

        public async Task<List<Order>> GetAsync()
        {
            return await Get<List<Order>>(nameof(Order));
        }
    }
}