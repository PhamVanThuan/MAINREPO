using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.DecisionTree
{
	[Serializable]
	public partial class PublishedVariableSetDataModel :  IDataModel
	{
		public PublishedVariableSetDataModel(Guid variableSetId, Guid publishStatusId, DateTime publishDate, string publisher)
		{
			this.VariableSetId = variableSetId;
			this.PublishStatusId = publishStatusId;
			this.PublishDate = publishDate;
			this.Publisher = publisher;
		
		}

		public PublishedVariableSetDataModel(Guid id, Guid variableSetId, Guid publishStatusId, DateTime publishDate, string publisher)
		{
			this.Id = id;
			this.VariableSetId = variableSetId;
			this.PublishStatusId = publishStatusId;
			this.PublishDate = publishDate;
			this.Publisher = publisher;
		
		}		

		public Guid Id { get; set; }

		public Guid VariableSetId { get; set; }

		public Guid PublishStatusId { get; set; }

		public DateTime PublishDate { get; set; }

		public string Publisher { get; set; }
	}
}