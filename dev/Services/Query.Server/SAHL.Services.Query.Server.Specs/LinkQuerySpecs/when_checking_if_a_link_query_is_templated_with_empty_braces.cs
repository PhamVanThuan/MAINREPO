using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAHL.Services.Query.Coordinators;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.Query.Server.Specs.LinkQuerySpecs
{
    public class when_checking_if_a_link_query_is_templated_with_empty_braces : WithFakes
    {

        Establish that = () =>
        {
            url = "{}";

            linkQuery = new LinkQuery(null, url, null, null);
        };


        private Because of = () =>
        {
            result = linkQuery.IsTemplatedUrl();
        };

        private It should_have_returned_false = () =>
        {
            result.ShouldBeFalse();
        };

        private static string url;
        private static LinkQuery linkQuery;
        private static bool result;
    }
}
