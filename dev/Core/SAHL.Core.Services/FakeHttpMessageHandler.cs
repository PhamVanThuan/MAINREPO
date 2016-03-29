using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Core.Services
{
    /// <summary>
    /// idea taken from : http://stackoverflow.com/questions/22239243/how-can-i-use-fakeiteasy-with-httpclient-in-a-unit-test
    /// To be used for testing purposes only to intercept the REQUEST and be able to return a fake RESPONSE.
    /// Usage :
    ///     const string json = "{\"success\": true}";
    ///     var messageHandler = FakeHttpMessageHandler.GetHttpMessageHandler(json, HttpStatusCode.BadRequest );
    ///     var httpClient = new HttpClient( messageHandler );
    /// </summary>
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private HttpResponseMessage _response;

        public HttpRequestMessage LastMessageSent { get; private set; }

        public string RequestContent { get; private set; }

        /// <summary>
        /// Creates an empty response with an HTTP response code of 200 (OK)
        /// </summary>
        public FakeHttpMessageHandler()
            : this(String.Empty, HttpStatusCode.OK)
        {
        }

        public FakeHttpMessageHandler(string content, HttpStatusCode code)
        {
            var memStream = new MemoryStream();
            var sw = new StreamWriter(memStream);
            sw.Write(content);
            sw.Flush();
            memStream.Position = 0;

            var httpContent = new StreamContent(memStream);

            _response = new HttpResponseMessage()
            {
                StatusCode = code,
                Content = httpContent
            };
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastMessageSent = request;
            RequestContent = request.Content.ReadAsStringAsync().Result;
            var tcs = new TaskCompletionSource<HttpResponseMessage>();
            tcs.SetResult(_response);
            return tcs.Task;
        }
    }
}