using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ApplicationDomain.Managers.Applicant.Statements
{
    public class DoesBankAccountBelongToApplicantOnApplicationStatement : ISqlStatement<int>
    {
        public int ApplicationNumber { get; protected set; }
        public int ClientBankAccountKey { get; protected set; }

        public DoesBankAccountBelongToApplicantOnApplicationStatement(int applicationNumber, int clientBankAccountKey)
        {
            this.ApplicationNumber = applicationNumber;
            this.ClientBankAccountKey = clientBankAccountKey;
        }

        public string GetStatement()
        {
            var query = @"SELECT 1 from offer o
                            INNER JOIN OfferRole r ON o.OfferKey = r.OfferKey
                            INNER JOIN OfferRoleType rt ON r.OfferRoleTypeKey = rt.OfferRoleTypeKey and rt.OfferRoleTypeGroupKey = 3
                            INNER JOIN LegalEntityBankAccount leba ON r.LegalEntityKey = leba.LegalEntityKey
                        WHERE o.OfferKey = @ApplicationNumber
                            AND leba.LegalEntityBankAccountKey = @ClientBankAccountKey";

            return query;
        }
    }
}
