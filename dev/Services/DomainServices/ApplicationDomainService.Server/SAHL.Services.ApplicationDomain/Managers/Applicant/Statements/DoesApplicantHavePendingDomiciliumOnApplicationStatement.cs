using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;

namespace SAHL.Services.ApplicationDomain.Managers.Applicant.Statements
{
    public class DoesApplicantHavePendingDomiciliumOnApplicationStatement : ISqlStatement<int>
    {
        public int ClientKey { get; protected set; }
        public int ApplicationNumber { get; protected set; }
        public int PendingStatusKey { get; protected set; }

        public DoesApplicantHavePendingDomiciliumOnApplicationStatement(int ClientKey, int ApplicationNumber)
        {
            this.ClientKey = ClientKey;
            this.ApplicationNumber = ApplicationNumber;
            this.PendingStatusKey = (int)GeneralStatus.Pending;
        }

        public string GetStatement()
        {
            var query = @"SELECT 
                            COUNT(*) AS Total
                        FROM
                            [2AM].[dbo].[OfferRole] OFR
                        JOIN
                            [2AM].[dbo].[OfferRoleDomicilium] ORD ON OFR.OfferRoleKey = ORD.OfferRoleKey
                        JOIN
                            [2AM].[dbo].[LegalEntityDomicilium] LED ON LED.LegalEntityDomiciliumKey = ORD.LegalEntityDomiciliumKey
                        WHERE
                            OFR.OfferKey = @ApplicationNumber
                        AND
                            OFR.LegalEntityKey = @ClientKey
                        AND
                            LED.GeneralStatusKey = @PendingStatusKey";
            return query;
        }
    }
}
