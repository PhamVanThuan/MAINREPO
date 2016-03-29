using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.DecisionTree
{
	[Serializable]
	public partial class DecisionTreeDataModel :  IDataModel
	{
		public DecisionTreeDataModel(string name, string description, bool isActive, string thumbnail)
		{
			this.Name = name;
			this.Description = description;
			this.IsActive = isActive;
			this.Thumbnail = thumbnail;
		
		}

		public DecisionTreeDataModel(Guid id, string name, string description, bool isActive, string thumbnail)
		{
			this.Id = id;
			this.Name = name;
			this.Description = description;
			this.IsActive = isActive;
			this.Thumbnail = thumbnail;
		
		}		

		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public bool IsActive { get; set; }

		public string Thumbnail { get; set; }
	}
}