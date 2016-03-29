using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
	[Serializable]
	public partial class InvitationDataModel :  IDataModel
	{
		public InvitationDataModel(Guid userId, Guid invitationToken, DateTime invitationDate, DateTime? acceptedDate)
		{
			this.UserId = userId;
			this.InvitationToken = invitationToken;
			this.InvitationDate = invitationDate;
			this.AcceptedDate = acceptedDate;
		
		}

		public InvitationDataModel(Guid id, Guid userId, Guid invitationToken, DateTime invitationDate, DateTime? acceptedDate)
		{
			this.Id = id;
			this.UserId = userId;
			this.InvitationToken = invitationToken;
			this.InvitationDate = invitationDate;
			this.AcceptedDate = acceptedDate;
		
		}		

		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public Guid InvitationToken { get; set; }

		public DateTime InvitationDate { get; set; }

		public DateTime? AcceptedDate { get; set; }
	}
}