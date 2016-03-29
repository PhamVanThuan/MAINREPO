using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
	[Serializable]
	public partial class PostOfficeDataModel :  IDataModel
	{
		public PostOfficeDataModel(int sAHLPostOfficeKey, string postOfficeName, string postalCode, Guid cityId)
		{
			this.SAHLPostOfficeKey = sAHLPostOfficeKey;
			this.PostOfficeName = postOfficeName;
			this.PostalCode = postalCode;
			this.CityId = cityId;
		
		}

		public PostOfficeDataModel(Guid id, int sAHLPostOfficeKey, string postOfficeName, string postalCode, Guid cityId)
		{
			this.Id = id;
			this.SAHLPostOfficeKey = sAHLPostOfficeKey;
			this.PostOfficeName = postOfficeName;
			this.PostalCode = postalCode;
			this.CityId = cityId;
		
		}		

		public Guid Id { get; set; }

		public int SAHLPostOfficeKey { get; set; }

		public string PostOfficeName { get; set; }

		public string PostalCode { get; set; }

		public Guid CityId { get; set; }
	}
}