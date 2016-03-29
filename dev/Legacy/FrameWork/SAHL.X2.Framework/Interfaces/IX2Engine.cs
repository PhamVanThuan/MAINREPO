using SAHL.Core.X2.Messages;
namespace SAHL.X2.Framework.Interfaces
{
    public interface IX2Engine
    {
        bool IsRunning { get; }

        void StartEngine(bool publishOnlyMode);

        void StopEngine();

        X2ResponseBase SendNodeManagmentMessage(IX2NodeManagementMessage message);

        X2ResponseBase SendLoginRequest(X2LoginRequest Request, string ADUserName);

        X2ResponseBase SendLogoutRequest(X2LogoutRequest Request, string ADUserName);

        X2ResponseBase SendCreateWorkFlowInstanceRequest(X2CreateWorkFlowInstanceRequest Request, string ADUserName);

        X2ResponseBase SendCreateWorkFlowInstanceWithCompleteRequest(X2CreateWorkFlowInstanceWithCompleteRequest Request, string ADUserName);

        X2ResponseBase SendActivityStartRequest(X2ActivityStartRequest Request, string ADUserName);

        X2ResponseBase SendActivityCompleteRequest(X2ActivityCompleteRequest Request, string ADUserName);

        X2ResponseBase SendActivityCreateCompleteRequest(X2ActivityCompleteRequest request, string adUserName);

        X2ResponseBase SendActivityCancelRequest(X2ActivityCancelRequest Request, string ADUserName);

        X2ResponseBase SendExternalActivityNotificationRequest();

        X2ResponseBase SendCommandRequest(string ADUser, X2CommandRequest request);

        X2ResponseBase SendProcessListRequest(string ADUser, X2RebuildWorklistRequest request);

        X2ResponseBase AquirePublisherMode(string ADUser, string ProcessName);

        X2ResponseBase ReleasePublisherMode(string ADUser);

        X2ResponseBase ResetScheduledEventTimer();

        X2ResponseBase ClearConnectionPool();
    }
}