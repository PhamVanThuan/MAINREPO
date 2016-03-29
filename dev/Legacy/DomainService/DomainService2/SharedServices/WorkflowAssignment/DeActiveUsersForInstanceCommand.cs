using System.Collections.Generic;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class DeActiveUsersForInstanceCommand : StandardDomainServiceCommand
    {
        public DeActiveUsersForInstanceCommand(long instanceID, int genericKey, List<string> dynamicRoles)
        {
            this.InstanceID = instanceID;
            this.GenericKey = genericKey;
            this.DynamicRoles = dynamicRoles;
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

        public bool Result { get; set; }
    }
}