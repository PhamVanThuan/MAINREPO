using SAHL.Core.Attributes;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    [NolockConventionExclude]
    public class DoesSAHLCityKeyExistQuery : ISqlStatement<CityDataModel>
    {
        public int SAHLCityKey { get; protected set; }

        public DoesSAHLCityKeyExistQuery(int sAHLCityKey)
        {
            this.SAHLCityKey = sAHLCityKey;
        }

        public string GetStatement()
        {
            return "SELECT Id,SAHLCityKey,CityName,ProvinceId FROM [Capitec].[geo].[City] (NOLOCK) WHERE SAHLCityKey = @SAHLCityKey";
        }
    }
}