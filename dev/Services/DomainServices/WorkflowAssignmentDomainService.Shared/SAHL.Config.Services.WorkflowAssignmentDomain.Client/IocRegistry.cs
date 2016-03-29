using System.Net.Http;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.WorkflowAssignmentDomain.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            const string serviceUrlConfigurationName = "WorkflowAssignmentDomainServiceUrlConfiguration";

            For<IServiceUrlConfigurationProvider>()
                .Use<ServiceUrlConfigurationProvider>().Named(serviceUrlConfigurationName)
                .Ctor<string>("serviceName").Is("WorkflowAssignmentDomainService");

            For<IWorkflowAssignmentDomainServiceClient>()
                .Use<WorkflowAssignmentDomainServiceClient>()
                .Ctor<IServiceUrlConfigurationProvider>().Is(x => x.TheInstanceNamed(serviceUrlConfigurationName))
                .Ctor<IJsonActivator>("jsonActivator").Is<JsonActivator>();
        }
    }
}
