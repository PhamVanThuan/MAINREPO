using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.DecisionTree
{
	[Serializable]
	public partial class DecisionTreeHistoryDataModel :  IDataModel
	{
		public DecisionTreeHistoryDataModel(Guid decisionTreeVersionId, string modificationUser, DateTime modificationDate)
		{
			this.DecisionTreeVersionId = decisionTreeVersionId;
			this.ModificationUser = modificationUser;
			this.ModificationDate = modificationDate;
		
		}

		public DecisionTreeHistoryDataModel(Guid id, Guid decisionTreeVersionId, string modificationUser, DateTime modificationDate)
		{
			this.Id = id;
			this.DecisionTreeVersionId = decisionTreeVersionId;
			this.ModificationUser = modificationUser;
			this.ModificationDate = modificationDate;
		
		}		

		public Guid Id { get; set; }

		public Guid DecisionTreeVersionId { get; set; }

		public string ModificationUser { get; set; }

		public DateTime ModificationDate { get; set; }
	}
}