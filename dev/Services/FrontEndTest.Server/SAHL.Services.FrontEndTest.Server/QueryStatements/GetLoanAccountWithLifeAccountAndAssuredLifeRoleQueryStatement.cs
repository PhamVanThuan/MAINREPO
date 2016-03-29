using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetLoanAccountWithLifeAccountAndAssuredLifeRoleQueryStatement : IServiceQuerySqlStatement<GetLoanAccountWithLifeAccountAndAssuredLifeRoleQuery, 
        GetLoanAccountWithLifeAccountAndAssuredLifeRoleQueryResult>
    {
        public string GetStatement()
        {
            var query = @"select top 1
                            p.AccountKey as LoanAccountKey, c.AccountKey as LifeAccountKey, le.LegalEntityKey
                            from [2AM].dbo.Account p
                            join [2AM].dbo.Account c on p.AccountKey = c.ParentAccountKey
                            join [2AM].dbo.Role r on c.AccountKey = r.AccountKey
                            join [2AM].dbo.LegalEntity le on r.LegalEntityKey = le.LegalEntityKey
                            where p.AccountStatusKey = 1
                            and c.AccountStatusKey = 1 and c.RRR_ProductKey = 4
                            and r.RoleTypeKey = 1 and r.GeneralStatusKey = 1
                            and c.AccountKey not in (select AccountKey from [2AM].dbo.DisabilityClaim where DisabilityClaimStatusKey in (1,3))
                            order by newid()";
            return query;
        }
    }
}