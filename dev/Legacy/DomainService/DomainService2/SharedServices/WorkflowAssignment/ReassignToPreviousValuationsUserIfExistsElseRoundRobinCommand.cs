namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReassignToPreviousValuationsUserIfExistsElseRoundRobinCommand : StandardDomainServiceCommand
    {
        public ReassignToPreviousValuationsUserIfExistsElseRoundRobinCommand(string dynamicRole, int organisationStructureKey, int applicationKey, string map, long instanceID, string state, int roundRobinPointerKey)
        {
            this.ApplicationKey = applicationKey;
            this.DynamicRole = dynamicRole;
            this.InstanceID = instanceID;
            this.OrganisationStructureKey = organisationStructureKey;
            this.RoundRobinPointerKey = roundRobinPointerKey;
            this.State = state;
            this.Map = map;
        }

        public string DynamicRole { get; set; }

        public int OrganisationStructureKey { get; set; }

        public int ApplicationKey { get; set; }

        public string Map { get; set; }

        public long InstanceID { get; set; }

        public string State { get; set; }

        public int RoundRobinPointerKey { get; set; }
    }
}