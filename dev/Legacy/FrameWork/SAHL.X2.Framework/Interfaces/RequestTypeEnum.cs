namespace SAHL.X2.Framework.Interfaces
{
    public enum RequestType
    {
        LoginRequest,
        LogoutRequest,
        CreateWorkFlowInstanceRequest,
        ActivityStartRequest,
        ActivityCompleteRequest,
        ActivityCancelRequest,
        ExternalActivityRequest,
        SystemRequest,
        SystemRequestGroup,
        CommandRequest,
        ProcessListRequest
    };
}