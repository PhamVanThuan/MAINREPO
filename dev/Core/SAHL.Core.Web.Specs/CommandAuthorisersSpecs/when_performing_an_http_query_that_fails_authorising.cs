using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.Services.Metrics;
using SAHL.Core.Web.Services;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace SAHL.Core.Web.Specs.CommandAuthorisersSpecs
{
    public class when_performing_an_http_query_that_fails_authorising : WithFakes
    {
        private static IPrincipal currentUser;
        private static IHostContext hostContext;
        private static IServiceQueryRouter serviceQueryRouter;
        private static ICommandServiceRequestMetrics commandServiceMetrics;
        private static IHttpCommandAuthoriser authoriser;
        private static IJsonActivator jsonQueryActivator;
        private static IQueryParameterManager queryParameterManager;
        private static ILogger logger;
        private static ILoggerSource loggerSource;
        private static QueryHttpHandlerBaseController queryController;
        private static ServiceQueryResult result;
        private static TestQueryWithAttributeWithRoles testQuery;
        private static HttpResponseMessage response;

        private Establish context = () =>
        {
            currentUser = new GenericPrincipal(new UnauthenticatedIdentity(), new string[] { });

            hostContext = An<IHostContext>();
            hostContext.WhenToldTo(x => x.GetUser()).Return(currentUser);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/capitecservice/queryservice/postquery");
            request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            request.Content = new StringContent("{\"name\":\"queryTest\"}", Encoding.UTF8, "application/json");

            request.Content.Headers.Add("CAPITECT-AUTH", "");

            testQuery = new TestQueryWithAttributeWithRoles();

            jsonQueryActivator = An<IJsonActivator>();
            jsonQueryActivator.WhenToldTo(x => x.DeserializeQuery(request.Content.ReadAsStringAsync().Result)).Return(testQuery);

            authoriser = new HttpCommandAuthoriser(hostContext);

            serviceQueryRouter = An<IServiceQueryRouter>();
            commandServiceMetrics = An<ICommandServiceRequestMetrics>();
            queryParameterManager = An<IQueryParameterManager>();
            logger = new FakeLogger();
            loggerSource = An<ILoggerSource>();

            queryController = new QueryHttpHandlerBaseController(serviceQueryRouter, commandServiceMetrics, authoriser, jsonQueryActivator, queryParameterManager, logger, loggerSource);

            queryController.Request = request;
        };

        private Because of = async () =>
        {
            response = await queryController.PerformHttpQuery(null, null) as HttpResponseMessage;
            result = response.Content.ReadAsAsync<ServiceQueryResult>().Result;
        };

        private It should_set_the_HasErrors_equal__to_true = () =>
        {
            result.SystemMessages.HasErrors.ShouldBeTrue();
        };

        private It should_have_an_ErrorMessages_collection_containing_items = () =>
        {
            result.SystemMessages.ErrorMessages().Any().ShouldBeTrue();
        };

        private It should_have_an_ErrorMessages_collection_containing_an_http_unauthorized_response_message_ = () =>
        {
            response.StatusCode.ShouldEqual<System.Net.HttpStatusCode>(System.Net.HttpStatusCode.Unauthorized);
        };
    }
}