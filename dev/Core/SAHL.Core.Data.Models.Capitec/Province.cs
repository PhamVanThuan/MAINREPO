using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
	[Serializable]
	public partial class ProvinceDataModel :  IDataModel
	{
		public ProvinceDataModel(int sAHLProvinceKey, string provinceName, Guid countryId)
		{
			this.SAHLProvinceKey = sAHLProvinceKey;
			this.ProvinceName = provinceName;
			this.CountryId = countryId;
		
		}

		public ProvinceDataModel(Guid id, int sAHLProvinceKey, string provinceName, Guid countryId)
		{
			this.Id = id;
			this.SAHLProvinceKey = sAHLProvinceKey;
			this.ProvinceName = provinceName;
			this.CountryId = countryId;
		
		}		

		public Guid Id { get; set; }

		public int SAHLProvinceKey { get; set; }

		public string ProvinceName { get; set; }

		public Guid CountryId { get; set; }
	}
}