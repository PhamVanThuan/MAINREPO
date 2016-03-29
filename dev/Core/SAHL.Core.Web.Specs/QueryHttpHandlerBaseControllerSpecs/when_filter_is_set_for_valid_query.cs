using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.Services.Metrics;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using System;
using System.Net.Http;

namespace SAHL.Core.Web.Specs.QueryHttpHandlerBaseControllerSpecs
{
    [Subject("SAHL.Core.Web.Services.QueryHttpHandlerBaseController.PerformHttpQuery")]
    public class when_filter_is_set_for_valid_query : WithFakes
    {
        private static string contentString = "";
        private static IServiceQuery query;

        private static int pageSizeValue = 10;
        private static int currentPageValue = 0;
        private static string filterOnValue = "test";
        private static string filterValue = "test";

        private static QueryHttpHandlerBaseController queryController;
        private static IServiceQueryRouter queryRouter;
        private static ICommandServiceRequestMetrics commandMetrics;
        private static IHttpCommandAuthoriser commandAuthoriser;
        private static IJsonActivator jsonActivator;
        private static IQueryParameterManager queryParameterManager;
        private static ILogger logger;
        private static ILoggerSource loggerSource;
        private static HttpResponseMessage result;

        Establish context = () =>
        {
            query = new FakeQuery(Guid.NewGuid());

            queryRouter = An<IServiceQueryRouter>();
            commandMetrics = An<ICommandServiceRequestMetrics>();
            commandAuthoriser = An<IHttpCommandAuthoriser>();
            jsonActivator = An<IJsonActivator>();
            queryParameterManager = An<IQueryParameterManager>();
            logger = new FakeLogger();
            loggerSource = An<ILoggerSource>();
            ISystemMessageCollection collection = SystemMessageCollection.Empty();

            HttpCommandAuthoriser.AuthToken authToken = new HttpCommandAuthoriser.AuthToken();

            commandAuthoriser.WhenToldTo(x => x.AuthoriseCommand(query)).Return(authToken);
            jsonActivator.WhenToldTo(x => x.DeserializeQuery(contentString)).Return(query);
            queryRouter.WhenToldTo(x => x.HandleQuery(query)).Return(collection);

            queryController = new QueryHttpHandlerBaseController(queryRouter, commandMetrics, commandAuthoriser, jsonActivator, queryParameterManager, logger, loggerSource);
            queryController.Request = new HttpRequestMessage();
            queryController.Request.Content = new FakeHttpContent(contentString);
        };

        Because of = async () =>
        {
            result = await queryController.PerformHttpQuery(pageSizeValue, currentPageValue, filterOnValue, filterValue);
        };

        It should_deserialize_query = () =>
        {
            jsonActivator.WasToldTo(x => x.DeserializeQuery(contentString));
        };

        It should_authorise_query = () =>
        {
            commandAuthoriser.WasToldTo(x => x.AuthoriseCommand(query));
        };

        It should_set_page_size_on_queryParameterManager = () =>
        {
            queryParameterManager.WasToldTo(y => y.SetParameter<PaginationQueryParameter>(Param<Action<PaginationQueryParameter>>.IsAnything));
        };

        It should_filters_on_queryParameterManager = () =>
        {
            queryParameterManager.WasToldTo(y => y.SetParameter<FilterQueryParameter>(Param<Action<FilterQueryParameter>>.IsAnything));
        };

        It should_not_order_by_on_queryParameterManager = () =>
        {
            queryParameterManager.WasNotToldTo(y => y.SetParameter<SortQueryParameter>(Param<Action<SortQueryParameter>>.IsAnything));
        };

        It should_increment_query_metrics = () =>
        {
            commandMetrics.WasToldTo(x => x.IncrementRequestForCommand<IServiceQuery>("QueryHttpHandleCommand"));
        };

        It should_tell_service_query_router_to_handle_query = () =>
        {
            queryRouter.WasToldTo(x => x.HandleQuery(Param<object>.IsAnything));
        };

        It should_update_latency_for_query = () =>
        {
            commandMetrics.WasToldTo(x => x.UpdateRequestLatencyForCommand<IServiceQuery>("QueryHttpHandleCommand", Param<long>.IsAnything));
        };
    }
}