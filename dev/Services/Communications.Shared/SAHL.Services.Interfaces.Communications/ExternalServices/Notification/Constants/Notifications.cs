namespace SAHL.Services.Interfaces.Communications.ExternalServices.Notification.Constants
{
    public class Notifications
    {
        public static string UpdateApplicationRecievedNotification(int applicationId)
        {
            return string.Format("Thank you for your application with SA Home Loans. A consultant will be in contact shortly. Application number: {0}", applicationId);
        }
    }
}