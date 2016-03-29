using System;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Commands
{
    public class CreateChildInstanceCommand : ServiceCommand
    {
        public InstanceDataModel Instance { get; set; }

        public Activity Activity { get; set; }

        public InstanceDataModel CreatedInstance { get; set; }

        public string WorkflowProviderName { get; protected set; }

        public String UserName { get; set; }

        public CreateChildInstanceCommand(InstanceDataModel parentInstance, Activity activity, string workflowProviderName, string userName)
        {
            this.Instance = parentInstance;
            this.Activity = activity;
            this.WorkflowProviderName = workflowProviderName;
            this.UserName = userName;
        }
    }
}