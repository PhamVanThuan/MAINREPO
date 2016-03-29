using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class ApplicationModuleDefinitionDataModel :  IDataModel, IDataModelWithIdentitySeed, IDataModelWithPrimaryKeyId 
    {
        public ApplicationModuleDefinitionDataModel(string name)
        {
            this.Name = name;
		
        }

        public ApplicationModuleDefinitionDataModel(int id, string name)
        {
            this.Id = id;
            this.Name = name;
		
        }		

        public int Id { get; set; }

        public string Name { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}