using System.Collections.Generic;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class DeActiveUsersForInstanceAndProcessCommand : StandardDomainServiceCommand
    {
        public DeActiveUsersForInstanceAndProcessCommand(long instanceID, int genericKey, List<string> dynamicRoles, SAHL.Common.Globals.Process process)
        {
            this.InstanceID = instanceID;
            this.GenericKey = genericKey;
            this.DynamicRoles = dynamicRoles;
            this.Process = process;
        }

        public long InstanceID
        {
            get;
            protected set;
        }

        public int GenericKey
        {
            get;
            protected set;
        }

        public List<string> DynamicRoles
        {
            get;
            protected set;
        }

        public SAHL.Common.Globals.Process Process
        {
            get;
            protected set;
        }
    }
}