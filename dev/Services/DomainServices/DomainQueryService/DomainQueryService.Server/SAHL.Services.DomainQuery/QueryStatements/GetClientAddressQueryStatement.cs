using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;

namespace SAHL.Services.DomainQuery.QueryStatements
{
    public class GetClientAddressQueryStatement : IServiceQuerySqlStatement<GetClientAddressQuery, GetClientAddressQueryResult>
    {
        public string GetStatement()
        {
            var query = @"SELECT lad.LegalEntityKey as ClientKey, lad.AddressKey, ad.AddressFormatKey  
                        FROM [2AM].[dbo].[LegalEntityAddress] lad
                            join [Address] ad on lad.AddressKey = ad.AddressKey
                        WHERE 
                            lad.LegalEntityAddressKey = @ClientAddressKey";

            return query;
        }
    }
}