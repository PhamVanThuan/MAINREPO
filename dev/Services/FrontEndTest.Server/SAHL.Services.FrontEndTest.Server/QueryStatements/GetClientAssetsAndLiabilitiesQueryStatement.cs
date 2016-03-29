using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetClientAssetsAndLiabilitiesQueryStatement : IServiceQuerySqlStatement<GetClientAssetsAndLiabilitiesQuery, GetClientAssetsAndLiabilitiesQueryResult>
    {
        public string GetStatement()
        {
            string query = @"select
                LegalEntityKey, AddressKey, AssetValue, LiabilityValue, CompanyName, Cost, Date, al.Description as AssetLiabilityDescription,
                alt.Description as AssetTypeDescription, AssetLiabilitySubTypeKey
                from [2am].dbo.LegalEntityAssetLiability lel
                join [2am].dbo.AssetLiability al on lel.AssetLiabilityKey = al.AssetLiabilityKey
                join [2am].dbo.AssetLiabilityType alt on al.AssetLiabilityTypeKey = alt.AssetLiabilityTypeKey
                where legalEntityKey = @ClientKey";
            return query;
        }
    }
}