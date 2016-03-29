using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.DecisionTree
{
	[Serializable]
	public partial class DecisionTreeVersionDataModel :  IDataModel
	{
		public DecisionTreeVersionDataModel(Guid decisionTreeId, int version, string data)
		{
			this.DecisionTreeId = decisionTreeId;
			this.Version = version;
			this.Data = data;
		
		}

		public DecisionTreeVersionDataModel(Guid id, Guid decisionTreeId, int version, string data)
		{
			this.Id = id;
			this.DecisionTreeId = decisionTreeId;
			this.Version = version;
			this.Data = data;
		
		}		

		public Guid Id { get; set; }

		public Guid DecisionTreeId { get; set; }

		public int Version { get; set; }

		public string Data { get; set; }
	}
}