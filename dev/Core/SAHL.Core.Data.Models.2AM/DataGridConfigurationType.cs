using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DataGridConfigurationTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DataGridConfigurationTypeDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public DataGridConfigurationTypeDataModel(int dataGridConfigurationTypeKey, string description)
        {
            this.DataGridConfigurationTypeKey = dataGridConfigurationTypeKey;
            this.Description = description;
		
        }		

        public int DataGridConfigurationTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.DataGridConfigurationTypeKey =  key;
        }
    }
}