using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ApplicationDomain.Managers.Applicant.Statements
{
    public class GetActiveClientRoleOnApplicationStatement : ISqlStatement<OfferRoleDataModel>
    {
        public int ApplicationNumber { get; protected set; }

        public int ClientKey { get; protected set; }

        public int ActiveStatusKey { get; protected set; }

        public int ClientOfferRoleTypeKey { get; protected set; }

        public GetActiveClientRoleOnApplicationStatement(int applicationNumber, int clientKey)
        {
            this.ApplicationNumber = applicationNumber;
            this.ClientKey = clientKey;
            this.ActiveStatusKey = (int)GeneralStatus.Active;
            this.ClientOfferRoleTypeKey = (int)OfferRoleTypeGroup.Client;
        }

        public string GetStatement()
        {
            return @"SELECT 
                        * 
                    FROM  
                        [2am].dbo.OfferRole OFR 
                    WHERE 
                        OFR.OfferRoleTypeKey IN (
                            SELECT 
                                OFRT.OfferRoleTypeKey 
                            FROM 
                                [2am].dbo.OfferRoleType OFRT 
                            WHERE OFRT.OfferRoleTypeGroupKey = @ClientOfferRoleTypeKey
                        ) 
                    AND 
                        OFR.GeneralStatusKey = @ActiveStatusKey 
                    AND 
                        OFR.OfferKey = @applicationNumber 
                    AND
                        OFR.LegalEntityKey = @ClientKey";
        }
    }
}