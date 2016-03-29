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
    public class when_retrieving_a_link_for_a_representation : WithFakes
    {
        Establish that = () =>
        {
            var bootstrapper = new ServiceBootstrapper();
            var container = bootstrapper.Initialise();

            linkResolver = container.GetInstance<ILinkResolver>();

            relativeUrlWithRelationship = "~/api/test2/{id}";

        };

        private Because of = () =>
        {
            link = linkResolver.GetLink(typeof (Test2Representation), false);
        };

        private It should_not_be_null = () =>
        {
            link.ShouldNotBeNull();
        };

        private It should_return_the_relative_current_request_url = () =>
        {
            link.Href.ShouldEqual(relativeUrlWithRelationship);
        };

        private It should_have_a_rel_named_Test2 = () =>
        {
            link.Rel.ShouldEqual("Test2");
        };

        private static ILinkResolver linkResolver;
        private static Link link;
        private static HttpRequest request;
        private static HttpResponse response;
        private static string relativeUrl;
        private static string relativeUrlWithRelationship;
    }
}
