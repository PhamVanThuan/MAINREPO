using System;
using System.Data;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.Factories;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.Query.Server.Specs.FactorySpecs
{
    public class when_constructing_a_web_request : WithFakes
    {
        Establish that = () =>
        {
            factory = new WebRequestFactory();

            url = "http://banana.url";
        };

        private Because of = () =>
        {
            request = factory.Create(url);
        };

        private It should_have_created_a_request = () =>
        {
            request.ShouldNotBeNull();
        };

        private It should_have_created_a_matching_web_request = () =>
        {
            request.RequestUri.ShouldEqual(new Uri(url));
        };

        private static WebRequestFactory factory;
        private static string url;
        private static WebRequest request;
    }
}
