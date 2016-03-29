using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.WorkflowTask;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.WorkflowTask.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IWebHttpClient>().Use<WebHttpClient>()
                .Ctor<HttpClientHandler>("handler").Is(new HttpClientHandler());

            const string serviceUrlConfigurationName = "WorkflowTaskServiceUrlConfiguration";

            For<IServiceUrlConfigurationProvider>()
                .Use<ServiceUrlConfigurationProvider>().Named(serviceUrlConfigurationName)
                .Ctor<string>("serviceName").Is("WorkflowTaskService");

            For<IWorkflowTaskServiceClient>()
                .Use<WorkflowTaskServiceClient>()
                .Ctor<IServiceUrlConfigurationProvider>().Is(a => a.TheInstanceNamed(serviceUrlConfigurationName))
                .Ctor<IJsonActivator>("jsonActivator").Is<JsonActivator>();

        }
    }
}
