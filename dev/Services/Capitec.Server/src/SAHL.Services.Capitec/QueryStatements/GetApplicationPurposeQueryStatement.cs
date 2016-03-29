using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.QueryStatements
{
	public class GetApplicationPurposeQueryStatement : IServiceQuerySqlStatement<GetApplicationPurposeQuery, GetApplicationPurposeQueryResult>
    {
        public string GetStatement()
        {
			return @"select Id, Name from [Capitec].dbo.ApplicationPurposeEnum where IsActive = 1";
        }
    }
}