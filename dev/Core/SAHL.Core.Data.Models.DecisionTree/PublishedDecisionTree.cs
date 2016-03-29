using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.DecisionTree
{
	[Serializable]
	public partial class PublishedDecisionTreeDataModel :  IDataModel
	{
		public PublishedDecisionTreeDataModel(Guid decisionTreeVersionId, Guid publishStatusId, DateTime publishDate, string publisher)
		{
			this.DecisionTreeVersionId = decisionTreeVersionId;
			this.PublishStatusId = publishStatusId;
			this.PublishDate = publishDate;
			this.Publisher = publisher;
		
		}

		public PublishedDecisionTreeDataModel(Guid id, Guid decisionTreeVersionId, Guid publishStatusId, DateTime publishDate, string publisher)
		{
			this.Id = id;
			this.DecisionTreeVersionId = decisionTreeVersionId;
			this.PublishStatusId = publishStatusId;
			this.PublishDate = publishDate;
			this.Publisher = publisher;
		
		}		

		public Guid Id { get; set; }

		public Guid DecisionTreeVersionId { get; set; }

		public Guid PublishStatusId { get; set; }

		public DateTime PublishDate { get; set; }

		public string Publisher { get; set; }
	}
}