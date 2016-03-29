using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.DecisionTree
{
	[Serializable]
	public partial class UserMRUDecisionTreeDataModel :  IDataModel
	{
		public UserMRUDecisionTreeDataModel(string userName, Guid treeId, DateTime modifiedDate, bool? pinned)
		{
			this.UserName = userName;
			this.TreeId = treeId;
			this.ModifiedDate = modifiedDate;
			this.Pinned = pinned;
		
		}

		public UserMRUDecisionTreeDataModel(Guid id, string userName, Guid treeId, DateTime modifiedDate, bool? pinned)
		{
			this.Id = id;
			this.UserName = userName;
			this.TreeId = treeId;
			this.ModifiedDate = modifiedDate;
			this.Pinned = pinned;
		
		}		

		public Guid Id { get; set; }

		public string UserName { get; set; }

		public Guid TreeId { get; set; }

		public DateTime ModifiedDate { get; set; }

		public bool? Pinned { get; set; }
	}
}