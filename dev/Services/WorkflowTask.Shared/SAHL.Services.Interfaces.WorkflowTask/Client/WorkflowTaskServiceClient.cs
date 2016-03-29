using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.WorkflowTask
{
    public class WorkflowTaskServiceClient : ServiceHttpClientWindowsAuthenticated, IWorkflowTaskServiceClient
    {
        private readonly IWebHttpClient webHttpClient;

        public WorkflowTaskServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator, IWebHttpClient webHttpClient)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
            this.webHttpClient = webHttpClient;
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IWorkflowTaskQuery
        {
            return PerformQueryInternal(query);
        }

        protected override IWebHttpClient GetConfiguredClient()
        {
            return this.webHttpClient != null ? webHttpClient : base.GetConfiguredClient();
        }
    }
}
