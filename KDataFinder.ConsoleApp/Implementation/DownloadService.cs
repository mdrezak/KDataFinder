using KDataFinder.ConsoleApp.Abstraction;
using System.Net;

namespace KDataFinder.ConsoleApp.Implementation
{
    internal class DownloadService : IDownloadService
    {
        private readonly CookieCollection _cookies;

        public DownloadService()
        {
            _cookies = new();
        }

        public async Task<byte[]> DownloadAsByteArray(string uri)
            => await (await Download(uri)).ReadAsByteArrayAsync();
        public async Task<string> DownloadAsString(string uri)
            => await (await Download(uri)).ReadAsStringAsync();
        public async Task<HttpContent> Download(string uriString)
        {
            Uri uri = new Uri(uriString);
            var handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            for (int i = _cookies.Count - 1; i >= 0; i--)
                _cookies[i].Domain = uri.Host;
            handler.CookieContainer.Add(_cookies);

            var httpClient = new HttpClient(handler);
            var response = await httpClient.GetAsync(uri);
            return response.Content;
        }
        public void SetAuthenticationCookie(string name, string value)
        {
            _cookies.Add(new Cookie(name, value, "/"));
        }
    }
}
