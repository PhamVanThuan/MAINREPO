using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
	[Serializable]
	public partial class BranchDataModel :  IDataModel
	{
		public BranchDataModel(string branchName, Guid suburbId, bool isActive, string branchCode)
		{
			this.BranchName = branchName;
			this.SuburbId = suburbId;
			this.IsActive = isActive;
			this.BranchCode = branchCode;
		
		}

		public BranchDataModel(Guid id, string branchName, Guid suburbId, bool isActive, string branchCode)
		{
			this.Id = id;
			this.BranchName = branchName;
			this.SuburbId = suburbId;
			this.IsActive = isActive;
			this.BranchCode = branchCode;
		
		}		

		public Guid Id { get; set; }

		public string BranchName { get; set; }

		public Guid SuburbId { get; set; }

		public bool IsActive { get; set; }

		public string BranchCode { get; set; }
	}
}