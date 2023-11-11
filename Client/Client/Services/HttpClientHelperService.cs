using Client.App.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using SharedApp.Validation;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Client.App.Services;

public class HttpClientHelperService : IHttpClientHelperService
{
    private readonly HttpClient _httpClient;
    private readonly IAccessTokenProvider _tokenProvider;

    public HttpClientHelperService(IHttpClientFactory httpClientFactory, IAccessTokenProvider tokenProvider)
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
    public async Task<T> Get<T>(string pathEndPoint, int id)
    {
        pathEndPoint = BuildUrlWithQueryParams(pathEndPoint, new(), id);
        var response = await _httpClient.GetAsync(pathEndPoint);
        return await ParseResponseAsync<T>(response);
    }

    public async Task<T> Get<T>(string pathEndPoint, Dictionary<string, string> parameters)
    {
        pathEndPoint = BuildUrlWithQueryParams(pathEndPoint, parameters);
        var response = await _httpClient.GetAsync(pathEndPoint);
        return await ParseResponseAsync<T>(response);
    }
    public async Task<T> Post<T>(string pathEndPoint)
    {
        var response = await _httpClient.PostAsJsonAsync(pathEndPoint, new StringContent(""));
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
    public async Task<string> Post(string pathEndPoint, Dictionary<string, string> data)
    {
        var response = await _httpClient.PostAsJsonAsync(pathEndPoint, data);
        return await ParseResponseAsync<string>(response);
    }
    public async Task<T> Put<T, U>(string pathEndPoint, int id, U data)
    {
        pathEndPoint = BuildUrlWithQueryParams(pathEndPoint, new(), id);
        var response = await _httpClient.PutAsJsonAsync(pathEndPoint, data);
        return await ParseResponseAsync<T>(response);
    }
    public async Task<T> Delete<T>(string pathEndPoint, int id)
    {
        pathEndPoint = BuildUrlWithQueryParams(pathEndPoint, new(), id);
        var response = await _httpClient.DeleteAsync(pathEndPoint);
        return await ParseResponseAsync<T>(response);
    }

    private async void ConfigureAuthorizationHeaderAsync()
    {
        var accessTokenResult = await _tokenProvider.RequestAccessToken();
        if (accessTokenResult.TryGetToken(out var accessToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken.Value);
        }
    }

    private static async Task<T> ParseResponseAsync<T>(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)(await httpResponseMessage.Content.ReadAsStringAsync())!;
            }
            return (await httpResponseMessage.Content.ReadFromJsonAsync<T>())!;
        }

        var problemDetail = await httpResponseMessage.Content.ReadFromJsonAsync<ProblemDetails>();
        throw new Exception(problemDetail!.Title);
    }

    private static string BuildUrlWithQueryParams(string url, Dictionary<string, string> queryParams, int? id = null)
    {
        if (id is not null) url += "/" + id;
        if (queryParams.Count == 0) return url;

        var queryString = string.Join("&", queryParams.Select(x => $"{x.Key}={WebUtility.UrlEncode(x.Value)}"));
        return $"{url}?{queryString}";
    }
}