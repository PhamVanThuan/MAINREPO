using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public interface IWebHttpClient
{
    Uri BaseAddress { get; set; }

    HttpRequestHeaders DefaultRequestHeaders { get; }

    Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);

    Task<HttpResponseMessage> Get(string requestUri);
}