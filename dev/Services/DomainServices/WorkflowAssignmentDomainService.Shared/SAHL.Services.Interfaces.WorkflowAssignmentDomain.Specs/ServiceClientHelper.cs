using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NSubstitute;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain;

namespace SAHL.Services.WorkflowTask.Shared.Tests
{
    public class ServiceClientHelper
    {
        internal static WorkflowAssignmentDomainServiceClient CreateServiceClient()
        {
            var urlProvider = Substitute.For<IServiceUrlConfigurationProvider>();
            var jsonActivator = new JsonActivator();

            var webHttpClient = Substitute.For<IWebHttpClient>();
            webHttpClient.PostAsync(Arg.Any<string>(), Arg.Any<HttpContent>())
                .Returns(a =>
                {
                    var taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();

                    var content = Substitute.For<HttpContent>();
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = content,
                        ReasonPhrase = "Completed successfully",
                    };

                    taskCompletionSource.SetResult(response);
                    return taskCompletionSource.Task;
                });

            var client = new WorkflowAssignmentDomainServiceClientForTesting(urlProvider, jsonActivator, webHttpClient);
            return client;
        }
    }

    internal class WorkflowAssignmentDomainServiceClientForTesting : WorkflowAssignmentDomainServiceClient
    {
        private readonly IWebHttpClient webHttpClient;

        public WorkflowAssignmentDomainServiceClientForTesting(IServiceUrlConfigurationProvider urlProvider, JsonActivator jsonActivator, IWebHttpClient webHttpClient) 
            : base(urlProvider, jsonActivator)
        {
            this.webHttpClient = webHttpClient;
        }

        protected override IWebHttpClient GetConfiguredClient()
        {
            return this.webHttpClient ?? base.GetConfiguredClient();
        }
    }
}