using System.Collections.Generic;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class AssignDebtCounsellingCaseForGroupOrLoadBalanceCommand : StandardDomainServiceCommand
    {
        public AssignDebtCounsellingCaseForGroupOrLoadBalanceCommand(long instanceID, int debtCounsellingKey, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, string state, List<string> states, System.Boolean includeStates, bool courtCase)
        {
            this.InstanceID = instanceID;
            this.DebtCounsellingKey = debtCounsellingKey;
            this.WorkflowRoleType = workflowRoleType;
            this.State = state;
            this.States = states;
            this.IncludeStates = includeStates;
            this.CourtCase = courtCase;
        }

        public long InstanceID
        {
            get;
            protected set;
        }

        public int DebtCounsellingKey
        {
            get;
            protected set;
        }

        public SAHL.Common.Globals.WorkflowRoleTypes WorkflowRoleType
        {
            get;
            protected set;
        }

        public string State
        {
            get;
            protected set;
        }

        public List<string> States
        {
            get;
            protected set;
        }

        public System.Boolean IncludeStates
        {
            get;
            protected set;
        }

        public bool CourtCase
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}