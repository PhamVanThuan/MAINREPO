using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class FormDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public FormDataModel(string name, string description, int? workFlowID)
        {
            this.Name = name;
            this.Description = description;
            this.WorkFlowID = workFlowID;
		
        }
		[JsonConstructor]
        public FormDataModel(int iD, string name, string description, int? workFlowID)
        {
            this.ID = iD;
            this.Name = name;
            this.Description = description;
            this.WorkFlowID = workFlowID;
		
        }		

        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? WorkFlowID { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}