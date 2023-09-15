namespace Client.App.Interfaces;

public interface IHttpClientHelper
{   
    Task<T> Get<T>(string pathEndPoint);
    Task<T> Get<T>(string pathEndPoint, Dictionary<string, string> parameters);
    Task<T> Post<T>(string pathEndPoint, T data);
    Task<T> Post<T>(string pathEndPoint, MultipartFormDataContent data);
    Task<T> Delete<T>(string pathEndPoint, int id);
}