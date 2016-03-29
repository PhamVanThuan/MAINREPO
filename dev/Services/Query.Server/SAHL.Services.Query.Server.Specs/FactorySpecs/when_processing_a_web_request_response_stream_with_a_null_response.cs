using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using NUnit.Framework;
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
    public class when_processing_a_web_request_response_stream_with_a_null_response : WithFakes
    {
        Establish that = () =>
        {
            response = An<WebResponse>();
            response.WhenToldTo(a => a.GetResponseStream())
                .Return((Stream)null);

            request = An<WebRequest>();
            request.WhenToldTo(a => a.GetResponse())
                .Return(response);

            factory = new StreamResultReaderFactory();
        };

        private Because of = () =>
        {
            result = factory.Process(request, a => a.ReadToEnd());
        };

        private It should_have_returned_an_empty_result = () =>
        {
            result.ShouldBeEmpty();
        };

        private static WebRequest request;
        private static StreamResultReaderFactory factory;
        private static WebResponse response;
        private static string result;
    }
}
