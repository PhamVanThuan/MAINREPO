using System.Collections.Generic;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class X2LoadBalanceAssignCommand : StandardDomainServiceCommand
    {
        public X2LoadBalanceAssignCommand(SAHL.Common.Globals.GenericKeyTypes userOrganisationStructureGenericKeyType, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, List<string> statesToDetermineLoad, SAHL.Common.Globals.Process process, SAHL.Common.Globals.Workflow workflow, System.Boolean checkRoundRobinStatus)
        {
            this.UserOrganisationStructureGenericKeyType = userOrganisationStructureGenericKeyType;
            this.WorkflowRoleType = workflowRoleType;
            this.GenericKey = genericKey;
            this.InstanceID = instanceID;
            this.StatesToDetermineLoad = statesToDetermineLoad;
            this.Process = process;
            this.Workflow = workflow;
            this.CheckRoundRobinStatus = checkRoundRobinStatus;
        }

        public SAHL.Common.Globals.GenericKeyTypes UserOrganisationStructureGenericKeyType
        {
            get;
            protected set;
        }

        public SAHL.Common.Globals.WorkflowRoleTypes WorkflowRoleType
        {
            get;
            protected set;
        }

        public int GenericKey
        {
            get;
            protected set;
        }

        public long InstanceID
        {
            get;
            protected set;
        }

        public List<string> StatesToDetermineLoad
        {
            get;
            protected set;
        }

        public SAHL.Common.Globals.Process Process
        {
            get;
            protected set;
        }

        public SAHL.Common.Globals.Workflow Workflow
        {
            get;
            protected set;
        }

        public System.Boolean CheckRoundRobinStatus
        {
            get;
            protected set;
        }

        public string Result
        {
            get;
            set;
        }
    }
}