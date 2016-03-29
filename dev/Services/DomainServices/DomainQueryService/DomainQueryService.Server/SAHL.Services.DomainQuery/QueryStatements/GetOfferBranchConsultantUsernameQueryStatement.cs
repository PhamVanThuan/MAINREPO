using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;

namespace SAHL.Services.DomainQuery.QueryStatements
{
    public class GetOfferBranchConsultantUsernameQueryStatement :
        IServiceQuerySqlStatement<GetOfferBranchConsultantUsernameQuery, GetOfferBranchConsultantUsernameQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT au.ADUserName AS BranchConsultantUsername FROM [2AM].dbo.OfferRole ore
                        JOIN [2AM].dbo.LegalEntity le ON le.LegalentityKey = ore.LegalEntityKey
                        JOIN [2AM].dbo.ADUser au ON au.LegalEntityKey = le.LegalEntityKey
                        WHERE ore.OfferKey = @OfferKey
                        AND ore.OfferRoleTypeKey = 101 --Branch consultant
                        AND ore.GeneralStatusKey = 1";
        }
    }
}