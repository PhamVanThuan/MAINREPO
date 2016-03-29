using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class ChangeCityDetailsQuery : ISqlStatement<CityDataModel>
    {
        public Guid Id { get; protected set; }

        public string CityName { get; protected set; }

        public int SAHLCityKey { get; protected set; }

        public Guid ProvinceId { get; protected set; }

        public ChangeCityDetailsQuery(Guid id, string cityName, int sahlCityKey, Guid provinceId)
        {
            this.Id = id;
            this.CityName = cityName;
            this.SAHLCityKey = sahlCityKey;
            this.ProvinceId = provinceId;
        }

        public string GetStatement()
        {
            return @"UPDATE [Capitec].[geo].[City]
SET [SAHLCityKey] = @SAHLCityKey,[CityName] = @CityName,[ProvinceId] = @ProvinceId
WHERE [Id] = @id";
        }
    }
}