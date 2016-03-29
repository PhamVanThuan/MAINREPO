using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.DecisionTree
{
	[Serializable]
	public partial class CurrentlyOpenDecisionTreeDataModel :  IDataModel
	{
		public CurrentlyOpenDecisionTreeDataModel(Guid decisionTreeVersionId, string username, DateTime openDate)
		{
			this.DecisionTreeVersionId = decisionTreeVersionId;
			this.Username = username;
			this.OpenDate = openDate;
		
		}

		public CurrentlyOpenDecisionTreeDataModel(Guid id, Guid decisionTreeVersionId, string username, DateTime openDate)
		{
			this.Id = id;
			this.DecisionTreeVersionId = decisionTreeVersionId;
			this.Username = username;
			this.OpenDate = openDate;
		
		}		

		public Guid Id { get; set; }

		public Guid DecisionTreeVersionId { get; set; }

		public string Username { get; set; }

		public DateTime OpenDate { get; set; }
	}
}