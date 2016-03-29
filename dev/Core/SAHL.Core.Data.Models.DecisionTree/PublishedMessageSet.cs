using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.DecisionTree
{
	[Serializable]
	public partial class PublishedMessageSetDataModel :  IDataModel
	{
		public PublishedMessageSetDataModel(Guid messageSetId, Guid publishStatusId, DateTime publishDate, string publisher)
		{
			this.MessageSetId = messageSetId;
			this.PublishStatusId = publishStatusId;
			this.PublishDate = publishDate;
			this.Publisher = publisher;
		
		}

		public PublishedMessageSetDataModel(Guid id, Guid messageSetId, Guid publishStatusId, DateTime publishDate, string publisher)
		{
			this.Id = id;
			this.MessageSetId = messageSetId;
			this.PublishStatusId = publishStatusId;
			this.PublishDate = publishDate;
			this.Publisher = publisher;
		
		}		

		public Guid Id { get; set; }

		public Guid MessageSetId { get; set; }

		public Guid PublishStatusId { get; set; }

		public DateTime PublishDate { get; set; }

		public string Publisher { get; set; }
	}
}