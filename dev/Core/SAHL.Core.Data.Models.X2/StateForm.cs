using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class StateFormDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public StateFormDataModel(int stateID, int formID, int formOrder)
        {
            this.StateID = stateID;
            this.FormID = formID;
            this.FormOrder = formOrder;
		
        }
		[JsonConstructor]
        public StateFormDataModel(int iD, int stateID, int formID, int formOrder)
        {
            this.ID = iD;
            this.StateID = stateID;
            this.FormID = formID;
            this.FormOrder = formOrder;
		
        }		

        public int ID { get; set; }

        public int StateID { get; set; }

        public int FormID { get; set; }

        public int FormOrder { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}