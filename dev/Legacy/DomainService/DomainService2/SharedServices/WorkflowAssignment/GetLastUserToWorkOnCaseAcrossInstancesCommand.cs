namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class GetLastUserToWorkOnCaseAcrossInstancesCommand : StandardDomainServiceCommand
    {
        public GetLastUserToWorkOnCaseAcrossInstancesCommand(long instanceID, long sourceInstanceID, int offerRoleTypeKey, string dynamicRole, string mapName)
        {
            this.InstanceID = instanceID;
            this.SourceInstanceID = sourceInstanceID;
            this.OfferRoleTypeKey = offerRoleTypeKey;
            this.DynamicRole = dynamicRole;
            this.MapName = mapName;
        }

        public long InstanceID { get; set; }

        public long SourceInstanceID { get; set; }

        public int OfferRoleTypeKey { get; set; }

        public string DynamicRole { get; set; }

        public string MapName { get; set; }

        public string ADUserNameResult { get; set; }
    }
}