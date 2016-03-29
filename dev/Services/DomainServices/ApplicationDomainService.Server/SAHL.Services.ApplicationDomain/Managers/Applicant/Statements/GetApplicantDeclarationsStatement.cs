using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Managers.Applicant.Statements
{
    public class GetApplicantDeclarationsStatement : ISqlStatement<OfferDeclarationDataModel>
    {
        public int ApplicationNumber { get; protected set; }

        public int ClientKey { get; protected set; }

        public GetApplicantDeclarationsStatement(int applicationNumber, int clientKey)
        {
            ApplicationNumber = applicationNumber;
            ClientKey = clientKey;
        }

        public string GetStatement()
        {
            string query = @"select od.* from [2am].dbo.OfferRole o 
                join [2am].dbo.OfferDeclaration od on o.offerRoleKey = od.offerRoleKey
                where o.LegalEntityKey = @ClientKey and o.OfferKey = @ApplicationNumber and o.generalStatusKey = 1 and o.offerRoleTypeKey in (8,10,11,12)";
            return query;
        }
    }
}