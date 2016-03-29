using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Models;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetClientAddressesQueryStatement : IServiceQuerySqlStatement<GetClientAddressesQuery, GetClientAddressesQueryResult>
    {
        public string GetStatement()
        {
            return @"select lea.*, a.addressFormatKey from [2am].dbo.LegalEntityAddress lea 
            join [2am].dbo.Address a on lea.addressKey=a.addressKey
            where LegalEntityKey = @ClientKey";
        }
    }
}