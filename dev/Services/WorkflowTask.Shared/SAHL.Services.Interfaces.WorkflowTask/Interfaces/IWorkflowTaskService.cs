using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.WorkflowTask
{
    public interface IWorkflowTaskService
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IWorkflowServiceCommand;
    }
}