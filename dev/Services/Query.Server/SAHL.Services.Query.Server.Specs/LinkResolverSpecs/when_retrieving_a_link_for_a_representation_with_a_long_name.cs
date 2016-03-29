using System;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web;
using SAHL.Config.Services;
using SAHL.Config.Services.Query.Server;
using SAHL.Services.Query.Server.Specs.Fakes;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;
using WebApi.Hal;

namespace SAHL.Services.Query.Server.Specs.LinkResolverSpecs
{
    public class when_retrieving_a_link_for_a_representation_with_a_long_name : WithFakes
    {
        Establish that = () =>
        {
            var bootstrapper = new ServiceBootstrapper();
            var container = bootstrapper.Initialise();

            linkResolver = container.GetInstance<ILinkResolver>();

            relativeUrl = "/api/tests/1";
            relativeUrlWithRelationship = relativeUrl + "/test2";

            request = new HttpRequest(string.Empty, "http://localhost" + relativeUrl, string.Empty);

            response = new HttpResponse(TextWriter.Null);
            HttpContext.Current = new HttpContext(request, response);
        };

        private Because of = () =>
        {
            link = linkResolver.GetLink(typeof (TestWithALongNameRepresentation), false);
        };

        private It should_have_a_rel_that_is_camel_cased = () =>
        {
            link.Rel.ShouldEqual("testWithALongName");
        };

        private static ILinkResolver linkResolver;
        private static Link link;
        private static HttpRequest request;
        private static HttpResponse response;
        private static string relativeUrl;
        private static string relativeUrlWithRelationship;
    }
}
