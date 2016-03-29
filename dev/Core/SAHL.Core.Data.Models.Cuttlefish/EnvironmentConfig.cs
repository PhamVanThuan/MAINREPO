using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class EnvironmentConfigDataModel :  IDataModel, IDataModelWithIdentitySeed, IDataModelWithPrimaryKeyId 
    {
        public EnvironmentConfigDataModel(string name, string queryAnalyserServerName, int? systemConfig_id)
        {
            this.Name = name;
            this.QueryAnalyserServerName = queryAnalyserServerName;
            this.SystemConfig_id = systemConfig_id;
		
        }

        public EnvironmentConfigDataModel(int id, string name, string queryAnalyserServerName, int? systemConfig_id)
        {
            this.Id = id;
            this.Name = name;
            this.QueryAnalyserServerName = queryAnalyserServerName;
            this.SystemConfig_id = systemConfig_id;
		
        }		

        public int Id { get; set; }

        public string Name { get; set; }

        public string QueryAnalyserServerName { get; set; }

        public int? SystemConfig_id { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}