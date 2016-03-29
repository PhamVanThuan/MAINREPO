using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.Notification.Default
{
    public class PendingDomiciliumNotificationMinorTileContentProvider : AbstractSqlTileContentProvider<PendingDomiciliumNotificationMinorTileModel>
    {
        public override string GetStatement(BusinessModel.BusinessKey businessKey)
        {
            return string.Format(@"SELECT 
		                                [2AM].dbo.LegalEntityLegalName(le.LegalEntityKey, 0)AS ClientName,
		                                le.IDNumber AS ClientID,
		                                DATEADD(MONTH, 1, led.ChangeDate )AS ExpiryDate,
		                                DATEDIFF(DAY,  GETDATE(), DATEADD(MONTH, 1, led.ChangeDate ))AS DaysLeft
                                    FROM
		                                [2AM].dbo.LegalEntityDomicilium led
		                                JOIN 
		                                LegalEntityAddress lea on led.LegalEntityAddressKey = lea.LegalEntityAddressKey
		                                JOIN 
		                                LegalEntity le on le.LegalEntityKey = lea.LegalEntityKey
	                                WHERE
		                                led.LegalEntityAddressKey = {0}", businessKey.Key);
            
        }
    }
}