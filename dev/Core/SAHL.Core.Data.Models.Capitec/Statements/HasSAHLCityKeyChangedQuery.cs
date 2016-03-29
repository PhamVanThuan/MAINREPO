using SAHL.Core.Attributes;
using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    [NolockConventionExclude]
    public class HasSAHLCityKeyChangedQuery : ISqlStatement<CityDataModel>
    {
        public Guid Id { get; protected set; }

        public int SAHLCityKey { get; protected set; }

        public HasSAHLCityKeyChangedQuery(Guid id, int sahlCityKey)
        {
            this.Id = id;
            this.SAHLCityKey = sahlCityKey;
        }

        public string GetStatement()
        {
            return "SELECT Id,SAHLCityKey,CityName,ProvinceId FROM [Capitec].[geo].[City] (NOLOCK) WHERE Id = @Id AND SAHLCityKey = @SAHLCityKey";
        }
    }
}