using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
    public class FilterBranchesByNameQueryStatement : IServiceQuerySqlStatement<FilterBranchesByNameQuery, FilterBranchesByNameQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT TOP 40 bra.Id,bra.BranchName
FROM [Capitec].[security].Branch bra
WHERE BranchName like @BranchName+'%' AND IsActive = 1";
        }
    }
}