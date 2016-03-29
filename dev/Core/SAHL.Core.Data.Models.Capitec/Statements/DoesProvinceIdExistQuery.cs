using SAHL.Core.Attributes;
using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    [NolockConventionExclude]
    public class DoesProvinceIdExistQuery : ISqlStatement<ProvinceDataModel>
    {
        public Guid ProvinceId { get; protected set; }

        public DoesProvinceIdExistQuery(Guid provinceId)
        {
            this.ProvinceId = provinceId;
        }

        public string GetStatement()
        {
            return "SELECT Id,SAHLProvinceKey,ProvinceName,CountryId FROM [Capitec].[geo].[Province] (NOLOCK) WHERE Id = @ProvinceId";
        }
    }
}