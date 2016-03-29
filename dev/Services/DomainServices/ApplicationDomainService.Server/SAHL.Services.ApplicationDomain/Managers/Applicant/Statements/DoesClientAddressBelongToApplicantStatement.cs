using SAHL.Core.Data;

namespace SAHL.Services.ApplicationDomain.Managers.Applicant.Statements
{
    public class DoesClientAddressBelongToApplicantStatement : ISqlStatement<int>
    {
        public int ClientAddressKey { get; protected set; }

        public int ApplicationNumber { get; protected set; }

        public DoesClientAddressBelongToApplicantStatement(int clientAddressKey, int applicationNumber)
        {
            this.ClientAddressKey = clientAddressKey;
            this.ApplicationNumber = applicationNumber;
        }

        public string GetStatement()
        {
            var sql = @"SELECT
                            O.[offerKey]
                        FROM
                            [2am].dbo.Offer O
                        JOIN
                            [2am].dbo.OfferRole OFR ON O.offerKey = OFR.OfferKey
                               AND
                                    OFR.OfferRoleTypeKey in (8,10,11,12)
                               AND
                                    OFR.GeneralStatusKey = 1
                        JOIN
                            [2am].dbo.LegalEntityAddress LEA ON OFR.LegalEntityKey = LEA.LegalEntityKey
                        WHERE
                            LEA.LegalEntityAddressKey = @clientAddressKey
                        AND
                            O.OfferKey = @applicationNumber";

            return sql;
        }
    }
}