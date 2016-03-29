using System.Collections.Generic;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReactivateUserOrLoadBalanceAssignCommand : StandardDomainServiceCommand
    {
        public ReactivateUserOrLoadBalanceAssignCommand(SAHL.Common.Globals.GenericKeyTypes userOrganisationStructureGenericType, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, List<string> statesToDetermineLoad, SAHL.Common.Globals.Process processName, SAHL.Common.Globals.Workflow workflowName)
        {
            this.UserOrganisationStructureGenericType = userOrganisationStructureGenericType;
            this.WorkflowRoleType = workflowRoleType;
            this.GenericKey = genericKey;
            this.InstanceID = instanceID;
            this.StatesToDetermineLoad = statesToDetermineLoad;
            this.ProcessName = processName;
            this.WorkflowName = workflowName;
        }

        public SAHL.Common.Globals.GenericKeyTypes UserOrganisationStructureGenericType
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

        public SAHL.Common.Globals.Process ProcessName
        {
            get;
            protected set;
        }

        public SAHL.Common.Globals.Workflow WorkflowName
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}