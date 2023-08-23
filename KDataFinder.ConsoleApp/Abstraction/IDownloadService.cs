
namespace KDataFinder.ConsoleApp.Abstraction;

internal interface IDownloadService
{
    Task<HttpContent> Download(string uri);
    void SetAuthenticationCookie(string name, string value);
    Task<byte[]> DownloadAsByteArray(string uri);
    Task<string> DownloadAsString(string uri);
}
