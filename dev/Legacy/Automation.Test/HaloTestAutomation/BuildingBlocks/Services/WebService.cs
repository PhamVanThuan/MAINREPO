using BuildingBlocks.Services.Contracts;
using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.Text;

namespace BuildingBlocks.Services
{
    public sealed class WebService : IWebService
    {
        public string MakeSOAPRequest(string soapXml, string soapActionUrl, Uri requestUri)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            request.Proxy = null;
            request.Method = "POST";
            request.ContentType = @"text/xml;charset=""utf-8""";
            request.KeepAlive = true;
            request.Headers.Add("SOAPAction", soapActionUrl);
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            using (var s = request.GetRequestStream())
            {
                using (var xmlWriter = new StreamWriter(s))
                    xmlWriter.Write(soapXml);
            }
            var response = (HttpWebResponse)request.GetResponse();

            using (var s = response.GetResponseStream())
            {
                using (var xmlReader = new StreamReader(s))
                    return xmlReader.ReadToEnd();
            }
        }
    }
}