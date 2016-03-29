using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class ApplicationConfigDataModel :  IDataModel, IDataModelWithIdentitySeed, IDataModelWithPrimaryKeyId 
    {
        public ApplicationConfigDataModel(string name, string description, int? systemConfig_id)
        {
            this.Name = name;
            this.Description = description;
            this.SystemConfig_id = systemConfig_id;
		
        }

        public ApplicationConfigDataModel(int id, string name, string description, int? systemConfig_id)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.SystemConfig_id = systemConfig_id;
		
        }		

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? SystemConfig_id { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}