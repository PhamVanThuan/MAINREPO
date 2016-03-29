using SAHL.Core.Services;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ClientDomain.Queries;

namespace SAHL.Services.ClientDomain.QueryStatements
{
    public class FindClientByIdNumberQueryStatement : IServiceQuerySqlStatement<FindClientByIdNumberQuery, ClientDetailsQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT TOP 1 LE.* FROM [2am].dbo.LegalEntity LE WHERE IDNumber = @IdNumber";
        }
    }
}
