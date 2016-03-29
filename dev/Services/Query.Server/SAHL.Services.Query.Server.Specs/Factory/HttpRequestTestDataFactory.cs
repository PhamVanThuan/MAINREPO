using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Hosting;
using SAHL.Services.Interfaces.Query.Parsers.Elements;
using SAHL.Services.Query.Parsers.Elemets;

namespace SAHL.Services.Query.Server.Specs.Factory
{
    public static class HttpRequestTestDataFactory
    {
        public static HttpRequestMessage GetAttorneyWhereHttpRequest()
        {
            string uri = @"http://localhost/api/attorneys?" + GetSimpleKey() + "=" + GetSimpleValue();
            return CreateMockRequest(HttpMethod.Get, uri);
        }

        public static HttpRequestMessage GetSimpleWhereHttpRequest()
        {
            string uri = @"http://localhost/api/attorneys?" + GetSimpleKey() + "=" + GetSimpleValue();
            return CreateMockRequest(HttpMethod.Get, uri);
        }

        public static HttpRequestMessage GetEmptyWhereHttpRequest()
        {
            string uri = @"http://localhost/api/someendpoint";
            return CreateMockRequest(HttpMethod.Get, uri);
        }

        private static HttpRequestMessage CreateMockRequest(HttpMethod method, string uri)
        {
            var request = new HttpRequestMessage(method, uri);
            request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            return request;
        }
        
        public static NameValueCollection GetSimpleWhereFilter()
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            nameValueCollection.Add(GetSimpleKey(), GetSimpleValue());
            return nameValueCollection;
        }
        
        public static NameValueCollection GetEmptyFilter()
        {
            return new NameValueCollection();
        }

        public static string GetSimpleKey()
        {
            return "filter[where][RegisteredName]";
        }

        public static string GetSimpleValue()
        {
            return "Simple_Value";
        }

    }

}