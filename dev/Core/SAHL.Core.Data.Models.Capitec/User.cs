using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
	[Serializable]
	public partial class UserDataModel :  IDataModel
	{
		public UserDataModel(string userName, string passwordHash, Guid securityStamp, bool isActive, bool isLockedOut)
		{
			this.UserName = userName;
			this.PasswordHash = passwordHash;
			this.SecurityStamp = securityStamp;
			this.IsActive = isActive;
			this.IsLockedOut = isLockedOut;
		
		}

		public UserDataModel(Guid id, string userName, string passwordHash, Guid securityStamp, bool isActive, bool isLockedOut)
		{
			this.Id = id;
			this.UserName = userName;
			this.PasswordHash = passwordHash;
			this.SecurityStamp = securityStamp;
			this.IsActive = isActive;
			this.IsLockedOut = isLockedOut;
		
		}		

		public Guid Id { get; set; }

		public string UserName { get; set; }

		public string PasswordHash { get; set; }

		public Guid SecurityStamp { get; set; }

		public bool IsActive { get; set; }

		public bool IsLockedOut { get; set; }
	}
}