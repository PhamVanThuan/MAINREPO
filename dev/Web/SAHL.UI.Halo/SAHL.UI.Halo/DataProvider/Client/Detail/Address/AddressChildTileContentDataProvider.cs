using SAHL.Core.BusinessModel;
using SAHL.Core.Data;

using SAHL.UI.Halo.Models.Common.LegalEntityAddress;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.ContentProvider.Client.Detail.Address
{
    public class AddressChildTileContentDataProvider : HaloTileBaseContentDataProvider<LegalEntityAddressChildModel>
    {
        public AddressChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"SELECT TOP 1
                                   CASE
                                        WHEN
                                            lea.AddressTypeKey = 2
                                        THEN 'Postal'
                                        ELSE 'Residential'
                                    END AS AddressType,
                                    [2AM].[dbo].[fGetFormattedAddressDelimited] (lea.AddressKey, 0) as Address,
                                    lea.EffectiveDate as EffectiveDate,
                                    CASE
                                        WHEN led.GeneralStatusKey = 1 THEN 'Yes'
                                        WHEN led.GeneralStatusKey = 3 THEN 'Pending'
                                        ELSE 'No'
                                    END AS IsDomicilium,
                                    CASE
                                        WHEN
                                            led.GeneralStatusKey = 3
                                            AND
                                            DATEDIFF(DAY, led.ChangeDate, GETDATE()) > 31
                                        THEN 'Pending domicilium expired'
                                        WHEN
                                            led.GeneralStatusKey = 3
                                            AND
                                            DATEDIFF(DAY, led.ChangeDate, GETDATE()) >= 5
                                        THEN 'Follow up with client about pending domicilium address'
                                        ELSE NULL
                                    END AS Notification
                                    FROM [2am].dbo.legalentityaddress lea
                                    LEFT JOIN [2AM].dbo.LegalEntityDomicilium led on led.LegalEntityAddressKey = lea.LegalEntityAddressKey
                                    WHERE
                                     lea.legalentityaddresskey = {0}
                                    ORDER BY led.ChangeDate DESC", businessContext.BusinessKey.Key);
        }
    }
}