using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class ChangeProvinceDetailsQuery : ISqlStatement<ProvinceDataModel>
    {
        public ChangeProvinceDetailsQuery(Guid provinceId, string provinceName, int sAHLProvinceKey, Guid countryId)
        {
            this.ProvinceId = provinceId;
            this.ProvinceName = provinceName;
            this.SAHLProvinceKey = sAHLProvinceKey;
            this.CountryId = countryId;
        }

        public Guid ProvinceId { get; protected set; }

        public string ProvinceName { get; protected set; }

        public int SAHLProvinceKey { get; protected set; }

        public Guid CountryId { get; protected set; }

        public string GetStatement()
        {
            return "UPDATE [Capitec].[geo].[Province] SET ProvinceName = @ProvinceName, SAHLProvinceKey = @SAHLProvinceKey,CountryId = @CountryId WHERE Id = @ProvinceId";
        }
    }
}