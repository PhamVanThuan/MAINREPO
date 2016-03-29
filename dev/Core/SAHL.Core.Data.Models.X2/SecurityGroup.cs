using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class SecurityGroupDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public SecurityGroupDataModel(bool isDynamic, string name, string description, int? processID, int? workFlowID)
        {
            this.IsDynamic = isDynamic;
            this.Name = name;
            this.Description = description;
            this.ProcessID = processID;
            this.WorkFlowID = workFlowID;
		
        }
		[JsonConstructor]
        public SecurityGroupDataModel(int iD, bool isDynamic, string name, string description, int? processID, int? workFlowID)
        {
            this.ID = iD;
            this.IsDynamic = isDynamic;
            this.Name = name;
            this.Description = description;
            this.ProcessID = processID;
            this.WorkFlowID = workFlowID;
		
        }		

        public int ID { get; set; }

        public bool IsDynamic { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? ProcessID { get; set; }

        public int? WorkFlowID { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}