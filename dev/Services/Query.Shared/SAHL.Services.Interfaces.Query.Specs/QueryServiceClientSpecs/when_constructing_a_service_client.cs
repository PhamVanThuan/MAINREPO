using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using NSubstitute;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.Query.Specs.Fakes;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.Interfaces.Query.Specs.QueryServiceClientSpecs
{
    public class when_constructing_a_service_client : WithFakes
    {

        Establish that = () =>
        {
            urlConfigurationprovider = An<IServiceUrlConfigurationProvider>();
            urlConfigurationprovider.WhenToldTo(a => a.ServiceName).Return("QueryService");
            urlConfigurationprovider.WhenToldTo(a => a.ServiceHostName).Return("nowhere");

            query = new QueryServiceQuery("somewhere/over/the/rainbow");

            json = @"{a:1,b:2}}";
            jsonActivator = An<IJsonActivator>();
            jsonResult = "{c:3,d:4}";
            queryServiceQueryResult = new QueryServiceQueryResult(jsonResult);
            jsonActivator
                .WhenToldTo(a => a.DeserializeObject<ServiceQueryResult>(Arg.Is(json)))
                .Return(new ServiceQueryResult
                {
                    ReturnData = queryServiceQueryResult,
                });

            webHttpClient = An<IWebHttpClient>();
            webHttpClient
                .PostAsync(Arg.Any<string>(), Arg.Any<HttpContent>())
                .Returns(a => {
                    
                    var taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();

                    var content = new StringContent(json);

                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = content,
                        ReasonPhrase = "Completed successfully",
                    };

                    taskCompletionSource.SetResult(response);
                    return taskCompletionSource.Task;
            });

            client = new QueryServiceClientForTesting(urlConfigurationprovider, jsonActivator, webHttpClient);
        };


        private Because of = () =>
        {
            result = client.HandleQuery(query);
        };

        private It should_have_retrieved_a_result = () =>
        {
            result.ShouldNotBeNull();
        };

        private It should_have_no_messages = () =>
        {
            result.AllMessages.ShouldBeEmpty();
        };

        private It should_have_a_query_result_set = () =>
        {
            queryServiceQueryResult.Result.ShouldEqual(jsonResult);
        };

        private It should_have_performed_an_http_request_on_the_expected_url = () =>
        {
            client.ObjectToSerialise.ShouldEqual(query);
        };

        private It should_have_the_supplied_relative_url_as_the_url_in_the_query_when_serialising = () =>
        {
            ((QueryServiceQuery) client.ObjectToSerialise).RelativeUrl.ShouldEqual(query.RelativeUrl);
        };

        private static QueryServiceClientForTesting client;
        private static IServiceUrlConfigurationProvider urlConfigurationprovider;
        private static IJsonActivator jsonActivator;
        private static QueryServiceQuery query;
        private static ISystemMessageCollection result;
        private static IWebHttpClient webHttpClient;
        private static TaskCompletionSource<string> contentTaskCompletionSource;
        private static string json;
        private static QueryServiceQueryResult queryServiceQueryResult;
        private static string jsonResult;
    }
}
