using SharedApp.Models;

namespace Client.App.Interfaces;

public interface IHttpClientHelper
{
    void ConfigureAuthorizationHeaderAsync();
    Task<List<T>> Get<T>(string pathEndPoint);
    Task<T> Post<T>(string pathEndPoint, T data);
    Task<ImageArtist> PostImageArtist(string pathEndPoint, MultipartFormDataContent data);
    Task<ImageCatalog> PostImageCatalog(string pathEndPoint, MultipartFormDataContent data);
}