using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.DecisionTree
{
	[Serializable]
	public partial class PublishedEnumerationSetDataModel :  IDataModel
	{
		public PublishedEnumerationSetDataModel(Guid enumerationSetId, Guid publishStatusId, DateTime publishDate, string publisher)
		{
			this.EnumerationSetId = enumerationSetId;
			this.PublishStatusId = publishStatusId;
			this.PublishDate = publishDate;
			this.Publisher = publisher;
		
		}

		public PublishedEnumerationSetDataModel(Guid id, Guid enumerationSetId, Guid publishStatusId, DateTime publishDate, string publisher)
		{
			this.Id = id;
			this.EnumerationSetId = enumerationSetId;
			this.PublishStatusId = publishStatusId;
			this.PublishDate = publishDate;
			this.Publisher = publisher;
		
		}		

		public Guid Id { get; set; }

		public Guid EnumerationSetId { get; set; }

		public Guid PublishStatusId { get; set; }

		public DateTime PublishDate { get; set; }

		public string Publisher { get; set; }
	}
}