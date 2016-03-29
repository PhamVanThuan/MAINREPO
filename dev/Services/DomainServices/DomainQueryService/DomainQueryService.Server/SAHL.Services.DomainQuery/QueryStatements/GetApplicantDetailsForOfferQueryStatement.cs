using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;

namespace SAHL.Services.DomainQuery.QueryStatements
{
    public class GetApplicantDetailsForOfferQueryStatement : IServiceQuerySqlStatement<GetApplicantDetailsForOfferQuery, GetApplicantDetailsForOfferQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT le.LegalEntityKey, le.FirstNames, le.Surname, le.IDNumber as IdentityNumber,
                            CAST(CASE WHEN ore.OfferRoleTypeKey IN (8, 11) THEN 1 ELSE 0 END AS BIT) AS IsMainApplicant
                            FROM [2AM].dbo.Offer o
                            JOIN [2AM].dbo.OfferRole ore ON o.OfferKey = ore.OfferKey
                            JOIN [2AM].dbo.LegalEntity le ON ore.LegalEntityKey = le.LegalEntityKey
                            JOIN [2AM].dbo.OfferRoleType ort ON ort.OfferRoleTypeKey = ore.OfferRoleTypeKey                         
                            WHERE o.OfferKey = @OfferKey
                            AND ort.OfferRoleTypeGroupKey = 3";
        }
    }
}