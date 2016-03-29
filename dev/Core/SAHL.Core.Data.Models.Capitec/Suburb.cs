using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
	[Serializable]
	public partial class SuburbDataModel :  IDataModel
	{
		public SuburbDataModel(int sAHLSuburbKey, string suburbName, string postalCode, Guid cityId)
		{
			this.SAHLSuburbKey = sAHLSuburbKey;
			this.SuburbName = suburbName;
			this.PostalCode = postalCode;
			this.CityId = cityId;
		
		}

		public SuburbDataModel(Guid id, int sAHLSuburbKey, string suburbName, string postalCode, Guid cityId)
		{
			this.Id = id;
			this.SAHLSuburbKey = sAHLSuburbKey;
			this.SuburbName = suburbName;
			this.PostalCode = postalCode;
			this.CityId = cityId;
		
		}		

		public Guid Id { get; set; }

		public int SAHLSuburbKey { get; set; }

		public string SuburbName { get; set; }

		public string PostalCode { get; set; }

		public Guid CityId { get; set; }
	}
}