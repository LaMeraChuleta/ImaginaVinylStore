using System.Net.Http.Headers;
using System.Net.Http.Json;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using SharedApp.Models;

namespace Client.App.Services;

public class HttpClientHelper : IHttpClientHelper
{
    private readonly HttpClient _httpClient;
    private readonly IAccessTokenProvider _tokenProvider;

    public HttpClientHelper(IHttpClientFactory httpClientFactory, IAccessTokenProvider tokenProvider)
    {
        _httpClient = httpClientFactory!.CreateClient("CatalogMusic.API");
        _tokenProvider = tokenProvider;

        ConfigureAuthorizationHeaderAsync();
    }

    public async void ConfigureAuthorizationHeaderAsync()
    {
        var accessTokenResult = await _tokenProvider.RequestAccessToken();
        if (accessTokenResult.TryGetToken(out var accessToken))
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken.Value);
    }

    public async Task<List<T>> Get<T>(string pathEndPoint)
    {
        return await _httpClient.GetFromJsonAsync<List<T>>(pathEndPoint) ?? throw new InvalidOperationException();
    }

    public async Task<T> Post<T>(string pathEndPoint, T data)
    {
        var response = await _httpClient.PostAsJsonAsync(pathEndPoint, data);
        return await response.Content.ReadFromJsonAsync<T>() ?? throw new InvalidOperationException();
    }

    public async Task<ImageArtist> PostImageArtist(string pathEndPoint, MultipartFormDataContent data)
    {
        var result = await _httpClient.PostAsync(pathEndPoint, data);
        return await result.Content.ReadFromJsonAsync<ImageArtist>() ?? throw new InvalidOperationException();
    }

    public async Task<ImageCatalog> PostImageCatalog(string pathEndPoint, MultipartFormDataContent data)
    {
        var result = await _httpClient.PostAsync(pathEndPoint, data);
        return await result.Content.ReadFromJsonAsync<ImageCatalog>() ?? throw new InvalidOperationException();
    }
}