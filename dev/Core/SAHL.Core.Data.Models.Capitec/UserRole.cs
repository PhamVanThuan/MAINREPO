using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
	[Serializable]
	public partial class UserRoleDataModel :  IDataModel
	{
		public UserRoleDataModel(Guid userId, Guid roleId)
		{
			this.UserId = userId;
			this.RoleId = roleId;
		
		}

		public UserRoleDataModel(Guid id, Guid userId, Guid roleId)
		{
			this.Id = id;
			this.UserId = userId;
			this.RoleId = roleId;
		
		}		

		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public Guid RoleId { get; set; }
	}
}