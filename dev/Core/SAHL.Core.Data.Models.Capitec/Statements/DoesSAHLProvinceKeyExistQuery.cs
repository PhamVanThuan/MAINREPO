using SAHL.Core.Attributes;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    [NolockConventionExclude]
    public class DoesSAHLProvinceKeyExistQuery : ISqlStatement<ProvinceDataModel>
    {
        public DoesSAHLProvinceKeyExistQuery(int sAHLProvinceKey)
        {
            this.SAHLProvinceKey = sAHLProvinceKey;
        }

        public int SAHLProvinceKey { get; protected set; }

        public string GetStatement()
        {
            return "SELECT Id, SAHLProvinceKey,ProvinceName,CountryId FROM [Capitec].[geo].[Province] (NOLOCK) WHERE SAHLProvinceKey = @SAHLProvinceKey";
        }
    }
}