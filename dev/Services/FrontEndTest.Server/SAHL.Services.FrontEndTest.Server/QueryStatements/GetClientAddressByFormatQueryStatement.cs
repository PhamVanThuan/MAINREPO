using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetClientAddressByFormatQueryStatement : IServiceQuerySqlStatement<GetClientAddressByFormatQuery, AddressDataModel>
    {
        public string GetStatement()
        {
            string query = @"select top 1 a.* from [FeTest].dbo.ClientAddresses ca
                            join [2am].dbo.Address a on ca.AddressKey = a.AddressKey
                            where ca.AddressFormatKey = @AddressFormat order by newid()";
            return query;
        }
    }
}