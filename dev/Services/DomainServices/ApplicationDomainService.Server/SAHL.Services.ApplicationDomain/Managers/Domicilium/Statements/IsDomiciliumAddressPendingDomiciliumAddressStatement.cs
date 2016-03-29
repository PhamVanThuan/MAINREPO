using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;

namespace SAHL.Services.ApplicationDomain.Managers.Domicilium.Statements
{
    public class IsDomiciliumAddressPendingDomiciliumAddressStatement : ISqlStatement<int>
    {
        public int ClientDomiciliumKey { get; protected set; }
        public int PendingStatusKey { get; protected set; }

        public IsDomiciliumAddressPendingDomiciliumAddressStatement(int ClientDomiciliumKey)
        {
            this.ClientDomiciliumKey = ClientDomiciliumKey;
            this.PendingStatusKey = (int)GeneralStatus.Pending;
        }

        public string GetStatement()
        {
            var query = @"SELECT 
                            COUNT(*) AS Total 
                        FROM
                            [2AM].[dbo].[LegalEntityDomicilium] LED
                        WHERE
                            LED.LegalEntityDomiciliumKey = @ClientDomiciliumKey
                        AND
                            LED.GeneralStatusKey = @PendingStatusKey";
            return query;
        }
    }
}
