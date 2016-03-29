using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
	[Serializable]
	public partial class UserInformationDataModel :  IDataModel
	{
		public UserInformationDataModel(Guid userId, string emailAddress, string firstName, string lastName)
		{
			this.UserId = userId;
			this.EmailAddress = emailAddress;
			this.FirstName = firstName;
			this.LastName = lastName;
		
		}

		public UserInformationDataModel(Guid id, Guid userId, string emailAddress, string firstName, string lastName)
		{
			this.Id = id;
			this.UserId = userId;
			this.EmailAddress = emailAddress;
			this.FirstName = firstName;
			this.LastName = lastName;
		
		}		

		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public string EmailAddress { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }
	}
}