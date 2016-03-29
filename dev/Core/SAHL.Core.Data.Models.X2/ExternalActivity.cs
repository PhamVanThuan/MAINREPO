using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class ExternalActivityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ExternalActivityDataModel(int workFlowID, string name, string description)
        {
            this.WorkFlowID = workFlowID;
            this.Name = name;
            this.Description = description;
		
        }
		[JsonConstructor]
        public ExternalActivityDataModel(int iD, int workFlowID, string name, string description)
        {
            this.ID = iD;
            this.WorkFlowID = workFlowID;
            this.Name = name;
            this.Description = description;
		
        }		

        public int ID { get; set; }

        public int WorkFlowID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}