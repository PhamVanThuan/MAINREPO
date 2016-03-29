using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class WorkFlowDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WorkFlowDataModel(int processID, int? workFlowAncestorID, string name, DateTime createDate, string storageTable, string storageKey, int iconID, string defaultSubject, int? genericKeyTypeKey)
        {
            this.ProcessID = processID;
            this.WorkFlowAncestorID = workFlowAncestorID;
            this.Name = name;
            this.CreateDate = createDate;
            this.StorageTable = storageTable;
            this.StorageKey = storageKey;
            this.IconID = iconID;
            this.DefaultSubject = defaultSubject;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }
		[JsonConstructor]
        public WorkFlowDataModel(int iD, int processID, int? workFlowAncestorID, string name, DateTime createDate, string storageTable, string storageKey, int iconID, string defaultSubject, int? genericKeyTypeKey)
        {
            this.ID = iD;
            this.ProcessID = processID;
            this.WorkFlowAncestorID = workFlowAncestorID;
            this.Name = name;
            this.CreateDate = createDate;
            this.StorageTable = storageTable;
            this.StorageKey = storageKey;
            this.IconID = iconID;
            this.DefaultSubject = defaultSubject;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }		

        public int ID { get; set; }

        public int ProcessID { get; set; }

        public int? WorkFlowAncestorID { get; set; }

        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public string StorageTable { get; set; }

        public string StorageKey { get; set; }

        public int IconID { get; set; }

        public string DefaultSubject { get; set; }

        public int? GenericKeyTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}