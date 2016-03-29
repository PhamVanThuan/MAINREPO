using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
	[Serializable]
	public partial class CityDataModel :  IDataModel
	{
		public CityDataModel(int sAHLCityKey, string cityName, Guid provinceId)
		{
			this.SAHLCityKey = sAHLCityKey;
			this.CityName = cityName;
			this.ProvinceId = provinceId;
		
		}

		public CityDataModel(Guid id, int sAHLCityKey, string cityName, Guid provinceId)
		{
			this.Id = id;
			this.SAHLCityKey = sAHLCityKey;
			this.CityName = cityName;
			this.ProvinceId = provinceId;
		
		}		

		public Guid Id { get; set; }

		public int SAHLCityKey { get; set; }

		public string CityName { get; set; }

		public Guid ProvinceId { get; set; }
	}
}