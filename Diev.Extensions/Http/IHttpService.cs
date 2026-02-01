namespace Diev.Extensions.Http;

public interface IHttpService
{
    Task<HttpResponseMessage> GetAsync(
        string uri,
        CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> PostAsync(
        string uri,
        HttpContent content,
        CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> PostAsJsonAsync<T>(
        string uri,
        T data,
        CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> PutAsync(
        string uri,
        HttpContent content,
        CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> PutAsJsonAsync<T>(
        string uri,
        T data,
        CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> DeleteAsync(
        string uri,
        CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken = default);
}
