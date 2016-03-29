namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class HandleApplicationManagementRolesOnReturnFromNTUtoPreviousStateCommand : StandardDomainServiceCommand
    {
        public HandleApplicationManagementRolesOnReturnFromNTUtoPreviousStateCommand(long instanceID, int applicationKey, string preNTUState, bool isFL, long applicationCaptureInstanceID, SAHL.Common.Globals.Process processName)
        {
            this.ApplicationCaptureInstanceID = applicationCaptureInstanceID;
            this.ApplicationKey = applicationKey;
            this.InstanceID = instanceID;
            this.IsFL = isFL;
            this.PreNTUState = preNTUState;
            this.ProcessName = processName;
        }

        public long InstanceID { get; set; }

        public int ApplicationKey { get; set; }

        public string PreNTUState { get; set; }

        public bool IsFL { get; set; }

        public long ApplicationCaptureInstanceID { get; set; }

        public SAHL.Common.Globals.Process ProcessName { get; set; }

        public string AssignedUserResult { get; set; }
    }
}