using SAHL.Core.Data;

namespace SAHL.Services.ApplicationDomain.Managers.Applicant.Statements
{
    public class CheckClientIsAnApplicantOnTheApplicationStatement : ISqlStatement<int>
    {
        public int ApplicationNumber { get; protected set; }

        public int ClientKey { get; protected set; }

        public CheckClientIsAnApplicantOnTheApplicationStatement(int clientKey, int applicationNumber)
        {
            ApplicationNumber = applicationNumber;
            ClientKey = clientKey;
        }

        public string GetStatement()
        {
            return @"SELECT
                        COUNT(*) AS Total
                    FROM
                        [2am].[dbo].[OfferRole] OFR
                    WHERE
                        OFR.OfferRoleTypeKey in (8,10,11,12)
                    AND
                        OFR.GeneralStatusKey = 1
                    AND
                        OFR.LegalEntityKey = @ClientKey
                    AND
                        OFR.OfferKey = @ApplicationNumber";
        }
    }
}