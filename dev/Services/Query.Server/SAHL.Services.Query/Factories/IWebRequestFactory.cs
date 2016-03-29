using System;
using System.Net;

namespace SAHL.Services.Query.Factories
{
    public interface IWebRequestFactory
    {
        WebRequest Create(Uri requestUri);
        WebRequest Create(string requestUriString);
    }
}