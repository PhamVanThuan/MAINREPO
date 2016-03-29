using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.DecisionTree
{
	[Serializable]
	public partial class DocumentTypeEnumDataModel :  IDataModel
	{
		public DocumentTypeEnumDataModel(string name)
		{
			this.Name = name;
		
		}

		public DocumentTypeEnumDataModel(Guid id, string name)
		{
			this.Id = id;
			this.Name = name;
		
		}		

		public Guid Id { get; set; }

		public string Name { get; set; }

		public const string TREE = "c6ba9e53-9487-46be-9440-a325008652d0";

		public const string ENUMERATION_SET = "ff5e3c92-2bd0-4ded-a4a9-a325008652d2";

		public const string MESSAGE_SET = "7f5c8a6a-2445-4a45-834c-a325008652d3";

		public const string VARIABLE_SET = "b862d8dc-50f6-4a17-b646-a325008652d4";
	}
}