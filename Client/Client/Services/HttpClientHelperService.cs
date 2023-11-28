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
    }
    public async Task<T> Get<T>(string pathEndPoint)
    {
        await ConfigureAuthorizationHeaderAsync();
        var response = await _httpClient.GetAsync(pathEndPoint);
        return await ParseResponseAsync<T>(response);
    }
    public async Task<T> Get<T>(string pathEndPoint, int id)
    {
        await ConfigureAuthorizationHeaderAsync();
        pathEndPoint = BuildUrlWithQueryParams(pathEndPoint, new(), id);
        var response = await _httpClient.GetAsync(pathEndPoint);
        return await ParseResponseAsync<T>(response);
    }
    public async Task<T> Get<T>(string pathEndPoint, Dictionary<string, string> parameters)
    {
        await ConfigureAuthorizationHeaderAsync();
        pathEndPoint = BuildUrlWithQueryParams(pathEndPoint, parameters);
        var response = await _httpClient.GetAsync(pathEndPoint);
        return await ParseResponseAsync<T>(response);
    }
    public async Task<T> Post<T>(string pathEndPoint, T data)
    {
        await ConfigureAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync(pathEndPoint, data);
        return await ParseResponseAsync<T>(response);
    }
    public async Task<T> Post<T>(string pathEndPoint, MultipartFormDataContent data)
    {
        await ConfigureAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsync(pathEndPoint, data);
        return await ParseResponseAsync<T>(response);
    }
    public async Task<string> Post(string pathEndPoint, object data)
    {
        await ConfigureAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync(pathEndPoint, data);
        return await ParseResponseAsync<string>(response);
    }
    public async Task<T> Put<T, U>(string pathEndPoint, int id, U data)
    {
        await ConfigureAuthorizationHeaderAsync();
        pathEndPoint = BuildUrlWithQueryParams(pathEndPoint, new(), id);
        var response = await _httpClient.PutAsJsonAsync(pathEndPoint, data);
        return await ParseResponseAsync<T>(response);
    }
    public async Task<T> Delete<T>(string pathEndPoint, int id)
    {
        await ConfigureAuthorizationHeaderAsync();
        pathEndPoint = BuildUrlWithQueryParams(pathEndPoint, new(), id);
        var response = await _httpClient.DeleteAsync(pathEndPoint);
        return await ParseResponseAsync<T>(response);
    }
    internal async Task ConfigureAuthorizationHeaderAsync()
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
        if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
        {
            return (T)(object)(new());
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