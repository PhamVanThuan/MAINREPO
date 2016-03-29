namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class GetBranchManagerOrgStructureKeyCommand : StandardDomainServiceCommand
    {
        public GetBranchManagerOrgStructureKeyCommand(long instanceID)
        {
            this.InstanceID = instanceID;
        }

        public long InstanceID { get; set; }

        public int OrganisationStructureKeyResult { get; set; }
    }
}