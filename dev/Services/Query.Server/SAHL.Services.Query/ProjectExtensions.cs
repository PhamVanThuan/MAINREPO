using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Hosting;
using SAHL.Services.Query.Serialiser;
using WebApi.Hal;

public static class ProjectExtensions
{
    public static IEnumerable<T> ToSingleItemEnumerable<T>(this T item)
    {
        yield return item;
    }

    public static string ToCamelCase(this string source)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return source;
        }
        if (source.Length == 1)
        {
            return char.IsUpper(source[0]) ? source.ToLower() : source;
        }
        return char.ToLower(source[0]) + source.Substring(1);
    }

    public static HttpResponseMessage ToHttpResponseMessage(this string content, HttpStatusCode status = HttpStatusCode.OK, Encoding encoding = null, string contentType = null)
    {
        var message = new HttpResponseMessage(status);
        message.Content = new StringContent(content, encoding ?? Encoding.UTF8, contentType ?? HalSerialiser.DefaultHalContentType);
        return message;
    }

    public static HttpResponseMessage ToHttpResponseMessage(this Representation representation, HttpRequestMessage request, IHalSerialiser serialiser, HttpStatusCode status = HttpStatusCode.OK)
    {
        var content = serialiser.Serialise(representation);
        var response = request.CreateResponse(status);
        response.Content = new StringContent(content, Encoding.UTF8, HalSerialiser.DefaultHalContentType);
        return response;
    }
}
