using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
	[Serializable]
	public partial class ActivityDataModel :  IDataModel
	{
		public ActivityDataModel(Guid userId, DateTime? lastLogin, DateTime? lastActivity)
		{
			this.UserId = userId;
			this.LastLogin = lastLogin;
			this.LastActivity = lastActivity;
		
		}

		public ActivityDataModel(Guid id, Guid userId, DateTime? lastLogin, DateTime? lastActivity)
		{
			this.Id = id;
			this.UserId = userId;
			this.LastLogin = lastLogin;
			this.LastActivity = lastActivity;
		
		}		

		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public DateTime? LastLogin { get; set; }

		public DateTime? LastActivity { get; set; }
	}
}