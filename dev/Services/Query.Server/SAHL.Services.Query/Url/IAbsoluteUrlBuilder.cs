using System;
using System.Web;

namespace SAHL.Services.Query
{
    public interface IAbsoluteUrlBuilder
    {
        string BuildPath(string relativeUrl, string applicationPath);
        string BuildUrl(string absolutePath, Uri currentRequestUri);
    }
}