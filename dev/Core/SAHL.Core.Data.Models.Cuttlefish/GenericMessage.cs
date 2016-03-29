using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class GenericMessageDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public GenericMessageDataModel(DateTime? messageDate, string messageContent, string messageType, int genericStatusID, int? batchID, int failureCount)
        {
            this.MessageDate = messageDate;
            this.MessageContent = messageContent;
            this.MessageType = messageType;
            this.GenericStatusID = genericStatusID;
            this.BatchID = batchID;
            this.FailureCount = failureCount;
		
        }

        public GenericMessageDataModel(int iD, DateTime? messageDate, string messageContent, string messageType, int genericStatusID, int? batchID, int failureCount)
        {
            this.ID = iD;
            this.MessageDate = messageDate;
            this.MessageContent = messageContent;
            this.MessageType = messageType;
            this.GenericStatusID = genericStatusID;
            this.BatchID = batchID;
            this.FailureCount = failureCount;
		
        }		

        public int ID { get; set; }

        public DateTime? MessageDate { get; set; }

        public string MessageContent { get; set; }

        public string MessageType { get; set; }

        public int GenericStatusID { get; set; }

        public int? BatchID { get; set; }

        public int FailureCount { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}