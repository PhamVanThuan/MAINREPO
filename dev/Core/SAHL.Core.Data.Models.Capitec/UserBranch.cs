using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
	[Serializable]
	public partial class UserBranchDataModel :  IDataModel
	{
		public UserBranchDataModel(Guid userId, Guid branchId)
		{
			this.UserId = userId;
			this.BranchId = branchId;
		
		}

		public UserBranchDataModel(Guid id, Guid userId, Guid branchId)
		{
			this.Id = id;
			this.UserId = userId;
			this.BranchId = branchId;
		
		}		

		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public Guid BranchId { get; set; }
	}
}