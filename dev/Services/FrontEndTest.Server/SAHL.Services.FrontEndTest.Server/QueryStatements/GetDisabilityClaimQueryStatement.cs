using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Queries;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetDisabilityClaimQueryStatement : IServiceQuerySqlStatement<GetDisabilityClaimQuery, DisabilityClaimDataModel>
    {
        public string GetStatement()
        {
            string query = @"select * from [2AM].[dbo].DisabilityClaim where DisabilityClaimKey = @DisabilityClaimKey";
            return query;
        }
    }
}