using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
	[Serializable]
	public partial class CountryDataModel :  IDataModel
	{
		public CountryDataModel(int sAHLCountryKey, string countryName)
		{
			this.SAHLCountryKey = sAHLCountryKey;
			this.CountryName = countryName;
		
		}

		public CountryDataModel(Guid id, int sAHLCountryKey, string countryName)
		{
			this.Id = id;
			this.SAHLCountryKey = sAHLCountryKey;
			this.CountryName = countryName;
		
		}		

		public Guid Id { get; set; }

		public int SAHLCountryKey { get; set; }

		public string CountryName { get; set; }
	}
}