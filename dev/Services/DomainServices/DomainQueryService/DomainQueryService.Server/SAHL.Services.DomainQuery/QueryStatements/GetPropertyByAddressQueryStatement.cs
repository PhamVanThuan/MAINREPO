using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;

namespace SAHL.Services.DomainQuery.QueryStatements
{
    public class GetPropertyByAddressQueryStatement : IServiceQuerySqlStatement<GetPropertyByAddressQuery, GetPropertyByAddressQueryResult>
    {
        public string GetStatement()
        {
            var query = @"SELECT p.*
                        FROM [2AM].[dbo].[Property] p
                        JOIN [2AM].[dbo].[Address] a on a.AddressKey = p.AddressKey
                        WHERE ISNULL(p.[ErfNumber], '') = ISNULL(@ErfNumber, '')
                        AND ISNULL(p.[ErfPortionNumber], '') = ISNULL(@ErfPortionNumber, '')
                        AND a.[AddressFormatKey] = 1
                        AND ISNULL(a.[UnitNumber],'') = ISNULL(@UnitNumber, '')
                        AND ISNULL(a.[BuildingNumber],'') = ISNULL(@BuildingNumber, '')
                        AND ISNULL(a.[BuildingName], '') = ISNULL(@BuildingName, '')
                        AND a.[StreetNumber] = @StreetNumber
                        AND a.[StreetName] = @StreetName
                        AND a.[RRR_SuburbDescription] = @Suburb
                        AND a.[RRR_CityDescription] = @City
                        AND ISNULL(a.[RRR_PostalCode], '') = ISNULL(@PostalCode, '')
                        AND a.[RRR_ProvinceDescription] = @Province";
            return query;
        }
    }
}