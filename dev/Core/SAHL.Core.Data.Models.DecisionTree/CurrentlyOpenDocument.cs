using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.DecisionTree
{
	[Serializable]
	public partial class CurrentlyOpenDocumentDataModel :  IDataModel
	{
		public CurrentlyOpenDocumentDataModel(Guid documentVersionId, string username, DateTime openDate, Guid documentTypeId)
		{
			this.DocumentVersionId = documentVersionId;
			this.Username = username;
			this.OpenDate = openDate;
			this.DocumentTypeId = documentTypeId;
		
		}

		public CurrentlyOpenDocumentDataModel(Guid id, Guid documentVersionId, string username, DateTime openDate, Guid documentTypeId)
		{
			this.Id = id;
			this.DocumentVersionId = documentVersionId;
			this.Username = username;
			this.OpenDate = openDate;
			this.DocumentTypeId = documentTypeId;
		
		}		

		public Guid Id { get; set; }

		public Guid DocumentVersionId { get; set; }

		public string Username { get; set; }

		public DateTime OpenDate { get; set; }

		public Guid DocumentTypeId { get; set; }
	}
}