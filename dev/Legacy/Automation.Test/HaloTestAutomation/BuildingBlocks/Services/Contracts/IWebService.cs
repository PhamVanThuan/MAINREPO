using System;

namespace BuildingBlocks.Services.Contracts
{
    public interface IWebService
    {
        string MakeSOAPRequest(string soapXml, string soapActionUrl, Uri requestUri);
    }
}