using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.PersonalLoan
{
    public class GetADUserNameByWorkflowRoleTypeCommand : StandardDomainServiceCommand
    {
        public GetADUserNameByWorkflowRoleTypeCommand(int applicationKey, int workflowRoleTypeKey)
        {
            this.ApplicationKey = applicationKey;
            this.WorkflowRoleTypeKey = workflowRoleTypeKey;
        }

        public int ApplicationKey { get; set; }

        public int WorkflowRoleTypeKey { get; set; }

        public string ADUserNameResult { get; set; }
    }
}