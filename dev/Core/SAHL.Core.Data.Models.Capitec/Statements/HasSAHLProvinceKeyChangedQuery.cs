using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class HasSAHLProvinceKeyChangedQuery : ISqlStatement<ProvinceDataModel>
    {
        public Guid Id { get; protected set; }

        public int SAHLProvinceKey { get; protected set; }

        public HasSAHLProvinceKeyChangedQuery(Guid id, int sahlProvinceKey)
        {
            this.Id = id;
            this.SAHLProvinceKey = sahlProvinceKey;
        }

        public string GetStatement()
        {
            return @"SELECT Id,SAHLProvinceKey,ProvinceName,CountryId
FROM [Capitec].[geo].[Province] WHERE Id = @Id AND SAHLProvinceKey = @SAHLProvinceKey";
        }
    }
}