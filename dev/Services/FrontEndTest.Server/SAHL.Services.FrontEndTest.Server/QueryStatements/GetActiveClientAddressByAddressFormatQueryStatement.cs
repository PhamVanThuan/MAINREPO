using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Queries;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetActiveClientAddressByAddressFormatQueryStatement : IServiceQuerySqlStatement<GetActiveClientAddressByAddressFormatQuery, LegalEntityAddressDataModel>
    {
        public string GetStatement()
        {
            string query = @"SELECT TOP 1 lea.* FROM [FETest].dbo.ClientAddresses ca
                            join [2am].dbo.LegalEntityAddress lea on ca.legalEntityAddressKey = lea.legalEntityAddressKey
                            where ca.AddressFormatKey = @AddressFormat
                            order by newid()";
            return query;
        }
    }
}