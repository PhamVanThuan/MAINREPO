using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.DecisionTree
{
	[Serializable]
	public partial class PublishStatusEnumDataModel :  IDataModel
	{
		public PublishStatusEnumDataModel(string name, bool isActive)
		{
			this.Name = name;
			this.IsActive = isActive;
		
		}

		public PublishStatusEnumDataModel(Guid id, string name, bool isActive)
		{
			this.Id = id;
			this.Name = name;
			this.IsActive = isActive;
		
		}		

		public Guid Id { get; set; }

		public string Name { get; set; }

		public bool IsActive { get; set; }

		public const string IN_PROGRESS = "f0b9774c-9d70-4a28-9079-a2d9008addf5";

		public const string PUBLISHED = "e9ca6370-7a94-48e1-a043-a2d9008aed40";
	}
}