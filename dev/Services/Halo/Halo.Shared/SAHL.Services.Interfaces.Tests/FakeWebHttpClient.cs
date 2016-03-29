using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.Tests
{
    public class FakeWebHttpClient : IWebHttpClient
    {
        public Uri BaseAddress { get; set; }
        public HttpRequestHeaders DefaultRequestHeaders { get; private set; }

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            var taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();

            var queryResult = new FakeQueryResult();

            var jsonActivator    = new JsonActivator();
            var serializedObject = jsonActivator.SerializeObject(queryResult);
            var stringContent    = new StringContent(serializedObject);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content      = stringContent,
                    ReasonPhrase = "Completed successfully",
                };

            taskCompletionSource.SetResult(httpResponseMessage);
            return taskCompletionSource.Task;
        }

        public Task<HttpResponseMessage> Get(string requestUri)
        {
            throw new NotImplementedException();
        }
    }
}
