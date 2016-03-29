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
    public class when_processing_a_web_request_response_stream : WithFakes
    {
        Establish that = () =>
        {
            streamContent = "banana";

            //using a MemoryStream so that I don't have to provide mock responses to all the stream calls
            stream = new MemoryStream(); 
            using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
            {
                writer.Write(streamContent);
                writer.Flush();
                writer.Close();
            }
            stream.Seek(0L, SeekOrigin.Begin);

            response = An<WebResponse>();
            response.WhenToldTo(a => a.GetResponseStream())
                .Return(stream);

            request = An<WebRequest>();
            request.WhenToldTo(a => a.GetResponse())
                .Return(response);

            factory = new StreamResultReaderFactory();

            //Stream processing pre-conditions
            stream.CanRead.ShouldBeTrue();
            stream.CanSeek.ShouldBeTrue();
            stream.CanWrite.ShouldBeTrue();
        };

        private Because of = () =>
        {
            result = factory.Process(request, a => a.ReadToEnd());
        };

        private It should_have_retrieved_the_response_for_the_request = () =>
        {
            request.WasToldTo(a => a.GetResponse()).OnlyOnce();
        };

        private It should_have_retrieved_the_response_stream = () =>
        {
            response.WasToldTo(a => a.GetResponseStream()).OnlyOnce();
        };

        private It should_have_closed_the_response_stream = () =>
        {
            stream.CanRead.ShouldBeFalse();
            stream.CanSeek.ShouldBeFalse();
            stream.CanWrite.ShouldBeFalse();
        };

        private It should_have_returned_the_value_retrieved_from_the_stream = () =>
        {
            result.ShouldEqual(streamContent);
        };

        private static WebRequest request;
        private static StreamResultReaderFactory factory;
        private static WebResponse response;
        private static Stream stream;
        private static string result;
        private static string streamContent;
    }
}
