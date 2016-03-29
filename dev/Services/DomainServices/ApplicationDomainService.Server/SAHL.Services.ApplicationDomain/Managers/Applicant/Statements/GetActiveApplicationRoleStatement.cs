using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ApplicationDomain.Managers.Applicant.Statements
{
    public class GetActiveApplicationRoleStatement : ISqlStatement<OfferRoleDataModel>
    {
        public int ApplicationNumber { get; protected set; }

        public int ClientKey { get; protected set; }

        public int ActiveStatusKey { get; protected set; }

        public GetActiveApplicationRoleStatement(int applicationNumber, int clientKey)
        {
            this.ApplicationNumber = applicationNumber;
            this.ClientKey = clientKey;
            this.ActiveStatusKey = (int)GeneralStatus.Active;
        }

        public string GetStatement()
        {
            var query = @"SELECT 
                            *
                        FROM
                            [2AM].[dbo].[OfferRole]
                        WHERE 
                            OfferKey = @ApplicationNumber
                        AND 
                            LegalEntityKey = @ClientKey
                        AND 
                            GeneralStatusKey = @ActiveStatusKey";
            return query;
        }
    }
}
