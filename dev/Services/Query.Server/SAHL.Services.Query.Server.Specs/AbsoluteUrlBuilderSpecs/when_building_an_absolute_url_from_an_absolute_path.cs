using System;
using System.Collections.Specialized;
using System.Data;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web;
using NSubstitute;
using SAHL.Core.Services;
using SAHL.Services.Query.UrlBuilders;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.Query.Server.Specs
{
    public class when_building_an_absolute_url_from_an_absolute_path : WithFakes
    {
        Establish that = () =>
        {
            relativeUrl = "/QueryService/somewhere/over/the/rainbow";

            urlConfig = An<IServiceUrlConfigurationProvider>();
            
            url = An<Uri>("http://banana.url");
            
            builder = new AbsoluteUrlBuilder(urlConfig);
        };

        private Because of = () =>
        {
            absoluteUrl = builder.BuildUrl(relativeUrl, url);
        };

        private It should_have_a_non_empty_absolute_url = () =>
        {
            absoluteUrl.ShouldNotBeEmpty();
        };

        private It should_return_the_expected_absolute_url = () =>
        {
            absoluteUrl.ShouldEqual(new Uri(url + relativeUrl.Substring(1)).ToString());
        };

        private static AbsoluteUrlBuilder builder;
        private static string relativeUrl;
        private static HttpContextBase context;
        private static string absoluteUrl;
        private static Exception exception;
        private static HttpRequestBase request;
        private static Uri url;
        private static HttpResponseBase response;
        private static string applicationPath;
        private static IServiceUrlConfigurationProvider urlConfig;
    }
}
