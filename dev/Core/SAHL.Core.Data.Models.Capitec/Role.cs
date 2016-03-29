using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
	[Serializable]
	public partial class RoleDataModel :  IDataModel
	{
		public RoleDataModel(string name)
		{
			this.Name = name;
		
		}

		public RoleDataModel(Guid id, string name)
		{
			this.Id = id;
			this.Name = name;
		
		}		

		public Guid Id { get; set; }

		public string Name { get; set; }
	}
}