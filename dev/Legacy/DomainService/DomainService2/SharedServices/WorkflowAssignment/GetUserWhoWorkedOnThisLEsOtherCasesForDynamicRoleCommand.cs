namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRoleCommand : StandardDomainServiceCommand
    {
        public GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRoleCommand(int applicationRoleTypeKey, int applicationKey, long instanceID)
        {
            this.ApplicationRoleTypeKey = applicationRoleTypeKey;
            this.ApplicationKey = applicationKey;
            this.InstanceID = instanceID;
        }

        public int ApplicationRoleTypeKey { get; set; }

        public int ApplicationKey { get; set; }

        public long InstanceID { get; set; }

        public string ADUserNameResult { get; set; }
    }
}