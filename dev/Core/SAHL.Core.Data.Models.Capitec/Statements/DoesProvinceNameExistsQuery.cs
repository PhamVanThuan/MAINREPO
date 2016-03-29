using SAHL.Core.Attributes;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    [NolockConventionExclude]
    public class DoesProvinceNameExistsQuery : ISqlStatement<ProvinceDataModel>
    {
        public string ProvinceName { get; protected set; }

        public DoesProvinceNameExistsQuery(string provinceName)
        {
            this.ProvinceName = provinceName;
        }

        public string GetStatement()
        {
            return "SELECT Id, SAHLProvinceKey,ProvinceName,CountryId FROM [Capitec].[geo].[Province] (NOLOCK) WHERE ProvinceName = @ProvinceName";
        }
    }
}