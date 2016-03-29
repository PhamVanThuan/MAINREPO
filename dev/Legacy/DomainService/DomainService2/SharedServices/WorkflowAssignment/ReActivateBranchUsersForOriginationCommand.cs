namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReActivateBranchUsersForOriginationCommand : StandardDomainServiceCommand
    {
        public ReActivateBranchUsersForOriginationCommand(long applicationManagementInstanceID, long applicationCaptureInstanceID, int applicationKey, string state, SAHL.Common.Globals.Process processName)
        {
            this.ApplicationCaptureInstanceID = applicationCaptureInstanceID;
            this.ApplicationManagementInstanceID = applicationManagementInstanceID;
            this.ApplicationKey = applicationKey;
            this.State = state;
            this.ProcessName = processName;
        }

        public long ApplicationManagementInstanceID { get; set; }

        public long ApplicationCaptureInstanceID { get; set; }

        public int ApplicationKey { get; set; }

        public string State { get; set; }

        public SAHL.Common.Globals.Process ProcessName { get; set; }

        public string AssignedUsersResult { get; set; }
    }
}