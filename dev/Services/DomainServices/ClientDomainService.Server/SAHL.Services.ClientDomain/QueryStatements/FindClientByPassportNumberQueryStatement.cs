using SAHL.Core.Services;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ClientDomain.Queries;

namespace SAHL.Services.ClientDomain.QueryStatements
{
    public class FindClientByPassportNumberQueryStatement : IServiceQuerySqlStatement<FindClientByPassportNumberQuery, ClientDetailsQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT TOP 1 LE.* FROM [2am].dbo.LegalEntity LE WHERE PassportNumber = @PassportNumber";
        }
    }
}
