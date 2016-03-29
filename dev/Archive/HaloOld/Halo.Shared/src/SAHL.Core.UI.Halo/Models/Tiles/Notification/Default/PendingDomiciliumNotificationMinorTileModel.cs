using SAHL.Core.UI.Models;
using System;

namespace SAHL.Core.UI.Halo.Tiles.Notification.Default
{
    public class PendingDomiciliumNotificationMinorTileModel : ITileModel
    {
        public string ClientName { get; set; }
        public string ClientID { get; set; }
        public DateTime ExpiryDate { get; set; }

        public string DaysLeft { get; set; }
    }
}