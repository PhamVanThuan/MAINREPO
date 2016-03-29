namespace SAHL.Services.Interfaces.Communications.ExternalServices.Notification
{
    public interface ISMSNotificationServiceConfiguration
    {
        string AffiliateCode { get; }

        string AuthenticationCode { get; }

        string MessageType { get; }

        string AppLinkUploadUrl { get; }

        string StartHour { get; }

        string EndHour { get; }

        bool UseRecipientNumber { get; }

        string TestRecipientNumber { get; }

        bool EnableNotifications { get; }
    }
}