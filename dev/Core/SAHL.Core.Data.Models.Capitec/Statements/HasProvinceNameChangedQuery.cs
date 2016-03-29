using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class HasProvinceNameChangedQuery : ISqlStatement<ProvinceDataModel>
    {
        public Guid Id { get; protected set; }

        public string ProvinceName { get; protected set; }

        public HasProvinceNameChangedQuery(Guid id, string provinceId)
        {
            this.Id = id;
            this.ProvinceName = provinceId;
        }

        public string GetStatement()
        {
            return @"SELECT Id,SAHLProvinceKey,ProvinceName,CountryId FROM [Capitec].[geo].[Province]
WHERE Id = @id AND ProvinceName = @ProvinceName";
        }
    }
}