using System;

namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class GetCityQueryResult
    {
        public Guid Id { get; set; }

        public string CityName { get; set; }

        public string SAHLCityKey { get; set; }

        public Guid ProvinceId { get; set; }
    }
}