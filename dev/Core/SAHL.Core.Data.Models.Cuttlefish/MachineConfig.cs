using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class MachineConfigDataModel :  IDataModel, IDataModelWithIdentitySeed, IDataModelWithPrimaryKeyId 
    {
        public MachineConfigDataModel(string name, int? environmentConfig_id)
        {
            this.Name = name;
            this.EnvironmentConfig_id = environmentConfig_id;
		
        }

        public MachineConfigDataModel(int id, string name, int? environmentConfig_id)
        {
            this.Id = id;
            this.Name = name;
            this.EnvironmentConfig_id = environmentConfig_id;
		
        }		

        public int Id { get; set; }

        public string Name { get; set; }

        public int? EnvironmentConfig_id { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}