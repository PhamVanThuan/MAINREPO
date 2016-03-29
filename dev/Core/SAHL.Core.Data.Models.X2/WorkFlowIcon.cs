using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class WorkFlowIconDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WorkFlowIconDataModel(string name, byte[] icon)
        {
            this.Name = name;
            this.Icon = icon;
		
        }
		[JsonConstructor]
        public WorkFlowIconDataModel(int iD, string name, byte[] icon)
        {
            this.ID = iD;
            this.Name = name;
            this.Icon = icon;
		
        }		

        public int ID { get; set; }

        public string Name { get; set; }

        public byte[] Icon { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}