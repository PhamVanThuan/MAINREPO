using System;
using System.IO;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Web;
using System.Web.WebPages;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.Web.Identity;

namespace SAHL.Services.Query.UrlBuilders
{
    public class AbsoluteUrlBuilder : IAbsoluteUrlBuilder
    {
        private readonly IServiceUrlConfigurationProvider configurationProvider;

        public AbsoluteUrlBuilder(IServiceUrlConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public string BuildPath(string relativeUrl, string applicationPath)
        {
            if (string.IsNullOrWhiteSpace(applicationPath))
            {
                return relativeUrl;
            }

            var applicationPathToUse = string.Empty;
            if (!relativeUrl.StartsWith(applicationPath, StringComparison.OrdinalIgnoreCase))
            {
                applicationPathToUse = applicationPath;
            }

            if (applicationPathToUse.IsEmpty())
            {
                return relativeUrl;
            }

            var relativeUrlToUse = PrepareRelativeUrl(relativeUrl);
            var absolutePath = VirtualPathUtility.ToAbsolute(relativeUrlToUse, applicationPathToUse);

            return absolutePath;
        }

        public string BuildUrl(string absolutePath, Uri currentRequestUri)
        {
            var port = GetPort(currentRequestUri);
            string hostName = string.IsNullOrWhiteSpace(configurationProvider.ServiceHostName) ? currentRequestUri.Host : configurationProvider.ServiceHostName;
            return string.Format("{0}://{1}{2}{3}",
                currentRequestUri.Scheme,
                hostName,
                port,
                absolutePath
                );
        }

        private static string PrepareRelativeUrl(string relativeUrl)
        {
            string relativeUrlToUse;
            if (relativeUrl.StartsWith("/"))
            {
                relativeUrlToUse = relativeUrl.Insert(0, "~");
            }
            else if (!relativeUrl.StartsWith("~/"))
            {
                relativeUrlToUse = relativeUrl.Insert(0, "~/");
            }
            else
            {
                relativeUrlToUse = relativeUrl;
            }
            return relativeUrlToUse;
        }

        private static string GetPort(Uri uri)
        {
            return uri.Port == 80 
                || (uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase) && uri.Port == 443) ? string.Empty : (":" + uri.Port);
        }
    }
}
