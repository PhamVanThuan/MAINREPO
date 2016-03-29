namespace DomainService2.SharedServices.Common
{
    public class HasInstancePerformedActivityCommand : StandardDomainServiceCommand
    {
        public HasInstancePerformedActivityCommand(long instanceID, string activity)
        {
            this.InstanceID = instanceID;
            this.Activity = activity;
        }

        public long InstanceID { get; protected set; }

        public string Activity { get; protected set; }

        public bool Result { get; set; }
    }
}