using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class ProcessAssemblyDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ProcessAssemblyDataModel(int processID, int? parentID, string dLLName, byte[] dLLData)
        {
            this.ProcessID = processID;
            this.ParentID = parentID;
            this.DLLName = dLLName;
            this.DLLData = dLLData;
		
        }
		[JsonConstructor]
        public ProcessAssemblyDataModel(int iD, int processID, int? parentID, string dLLName, byte[] dLLData)
        {
            this.ID = iD;
            this.ProcessID = processID;
            this.ParentID = parentID;
            this.DLLName = dLLName;
            this.DLLData = dLLData;
		
        }		

        public int ID { get; set; }

        public int ProcessID { get; set; }

        public int? ParentID { get; set; }

        public string DLLName { get; set; }

        public byte[] DLLData { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}