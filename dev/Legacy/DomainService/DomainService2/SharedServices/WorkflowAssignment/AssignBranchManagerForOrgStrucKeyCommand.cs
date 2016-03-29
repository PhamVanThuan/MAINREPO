namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class AssignBranchManagerForOrgStrucKeyCommand : StandardDomainServiceCommand
    {
        public long InstanceID { get; set; }

        public string DynamicRole { get; set; }

        public int OrganisationStructureKey { get; set; }

        public int GenericKey { get; set; }

        public string State { get; set; }

        public SAHL.Common.Globals.Process Process { get; set; }

        public AssignBranchManagerForOrgStrucKeyCommand(long instanceID, string dynamicRole, int organisationStructureKey, int genericKey, string state, SAHL.Common.Globals.Process process)
        {
            this.InstanceID = instanceID;
            this.DynamicRole = dynamicRole;
            this.OrganisationStructureKey = organisationStructureKey;
            this.GenericKey = genericKey;
            this.State = state;
            this.Process = process;
        }

        public string AssignedManagerResult { get; set; }
    }
}