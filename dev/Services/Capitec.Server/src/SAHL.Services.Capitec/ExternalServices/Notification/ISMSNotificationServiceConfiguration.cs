using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.ExternalServices.Notification
{
    public interface ISMSNotificationServiceConfiguration
    {
        string AffiliateCode { get; }

        string AuthenticationCode { get; }
        
        string MessageType { get; }

        string AppLinkUploadUrl { get;  }

        string StartHour { get; }

        string EndHour { get; }

        bool UseRecipientNumber { get; }

        string TestRecipientNumber { get; }

        bool EnableNotifications { get; }
    }
}
