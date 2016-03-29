using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.DecisionTree
{
	[Serializable]
	public partial class EnumerationSetDataModel :  IDataModel
	{
		public EnumerationSetDataModel(int version, string data)
		{
			this.Version = version;
			this.Data = data;
		
		}

		public EnumerationSetDataModel(Guid id, int version, string data)
		{
			this.Id = id;
			this.Version = version;
			this.Data = data;
		
		}		

		public Guid Id { get; set; }

		public int Version { get; set; }

		public string Data { get; set; }
	}
}