using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Azure;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using SharedApp.Validation;

namespace Client.App.Services;

public class HttpClientHelper : IHttpClientHelper
{
    private readonly HttpClient _httpClient;
    private readonly IAccessTokenProvider _tokenProvider;

    public HttpClientHelper(IHttpClientFactory httpClientFactory, IAccessTokenProvider tokenProvider)
    {
        _httpClient = httpClientFactory.CreateClient("CatalogMusic.API");
        _tokenProvider = tokenProvider;

        ConfigureAuthorizationHeaderAsync();
    }

    public async Task<T> Get<T>(string pathEndPoint)
    {
        var response = await _httpClient.GetAsync(pathEndPoint);
        return await ParseResponseAsync<T>(response);
    }

    public async Task<T> Get<T>(string pathEndPoint, Dictionary<string, string> parameters)
    {
        pathEndPoint = BuildUrlWithQueryParams(pathEndPoint, parameters);
        var response = await _httpClient.GetAsync(pathEndPoint);
        return await ParseResponseAsync<T>(response);
    }

    public async Task<T> Post<T>(string pathEndPoint, T data)
    {
        var response = await _httpClient.PostAsJsonAsync(pathEndPoint, data);
        return await ParseResponseAsync<T>(response);
    }

    public async Task<T> Post<T>(string pathEndPoint, MultipartFormDataContent data)
    {
        var response = await _httpClient.PostAsync(pathEndPoint, data);
        return await ParseResponseAsync<T>(response);
    }
    public async Task<T> Delete<T>(string pathEndPoint, int id)
    {
        var response = await _httpClient.DeleteAsync(pathEndPoint + "/" + id.ToString());
        return await ParseResponseAsync<T>(response);
    }

    private async void ConfigureAuthorizationHeaderAsync()
    {
        var accessTokenResult = await _tokenProvider.RequestAccessToken();
        if (accessTokenResult.TryGetToken(out var accessToken))
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken.Value);
    }

    private static async Task<T> ParseResponseAsync<T>(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            return (await httpResponseMessage.Content.ReadFromJsonAsync<T>())!;

        var problemDetail = await httpResponseMessage.Content.ReadFromJsonAsync<ProblemDetails>();
        throw new Exception(problemDetail!.Title);
    }

    private static string BuildUrlWithQueryParams(string url, Dictionary<string, string> queryParams)
    {
        if (queryParams.Count == 0) return url;

        var queryString = string.Join("&", queryParams.Select(x => $"{x.Key}={WebUtility.UrlEncode(x.Value)}"));
        return $"{url}?{queryString}";
    }
}