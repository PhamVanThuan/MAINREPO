using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.Notification.Default
{
    public class PendingDomiciliumNotificationMinorTileDataProvider : AbstractSqlTileDataProvider, ITileDataProvider<PendingDomiciliumNotificationMinorTileModel>
    {
        public override string GetStatement(BusinessKey businessKey)
        {
            return string.Format(@"SELECT
	                                lea.LegalEntityAddressKey AS BusinessKey,
	                                0 AS BusinessKeyType
	                                FROM
	                                [2AM].dbo.LegalEntity le
	                                JOIN
	                                [2AM].dbo.LegalEntityAddress lea ON lea.LegalEntityKey = le.LegalEntityKey
	                                JOIN
	                                [2AM].dbo.LegalEntityDomicilium led ON led.LegalEntityAddressKey = lea.LegalEntityAddressKey
                                WHERE
	                                le.LegalEntityKey = {0}
	                                AND
	                                led.GeneralStatusKey = 3 -- Pending
	                                AND
	                                GETDATE() >= DATEADD(DAY, 5, led.ChangeDate )
	                                AND
	                                GETDATE() <= DATEADD(MONTH, 1, led.ChangeDate )", businessKey.Key);
        }
    }
}