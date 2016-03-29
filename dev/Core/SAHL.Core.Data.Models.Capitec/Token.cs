using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
	[Serializable]
	public partial class TokenDataModel :  IDataModel
	{
		public TokenDataModel(Guid userId, Guid securityToken, string ipAddress, DateTime issueDate)
		{
			this.UserId = userId;
			this.SecurityToken = securityToken;
			this.IpAddress = ipAddress;
			this.IssueDate = issueDate;
		
		}

		public TokenDataModel(Guid id, Guid userId, Guid securityToken, string ipAddress, DateTime issueDate)
		{
			this.Id = id;
			this.UserId = userId;
			this.SecurityToken = securityToken;
			this.IpAddress = ipAddress;
			this.IssueDate = issueDate;
		
		}		

		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public Guid SecurityToken { get; set; }

		public string IpAddress { get; set; }

		public DateTime IssueDate { get; set; }
	}
}