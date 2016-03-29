namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class GetLatestUserAcrossInstancesCommand : StandardDomainServiceCommand
    {
        public GetLatestUserAcrossInstancesCommand(long instanceID, int applicationKey, int organisationStructureKey, string dynamicRole, string state, SAHL.Common.Globals.Process process)
        {
            this.InstanceID = instanceID;
            this.OrganisationStructureKey = organisationStructureKey;
            this.DynamicRole = dynamicRole;
            this.State = state;
            this.Process = process;
            this.ApplicationKey = applicationKey;
        }

        public long InstanceID { get; set; }

        public int ApplicationKey { get; set; }

        public int OrganisationStructureKey { get; set; }

        public string DynamicRole { get; set; }

        public string State { get; set; }

        public SAHL.Common.Globals.Process Process { get; set; }

        public string ADUserNameResult { get; set; }
    }
}