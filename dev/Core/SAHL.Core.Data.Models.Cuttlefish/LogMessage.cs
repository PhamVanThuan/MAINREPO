using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class LogMessageDataModel :  IDataModel, IDataModelWithIdentitySeed, IDataModelWithPrimaryKeyId 
    {
        public LogMessageDataModel(DateTime? messageDate, string logMessageType, string methodName, string message, string source, string userName, string machineName, string application)
        {
            this.MessageDate = messageDate;
            this.LogMessageType = logMessageType;
            this.MethodName = methodName;
            this.Message = message;
            this.Source = source;
            this.UserName = userName;
            this.MachineName = machineName;
            this.Application = application;
		
        }

        public LogMessageDataModel(int id, DateTime? messageDate, string logMessageType, string methodName, string message, string source, string userName, string machineName, string application)
        {
            this.Id = id;
            this.MessageDate = messageDate;
            this.LogMessageType = logMessageType;
            this.MethodName = methodName;
            this.Message = message;
            this.Source = source;
            this.UserName = userName;
            this.MachineName = machineName;
            this.Application = application;
		
        }		

        public int Id { get; set; }

        public DateTime? MessageDate { get; set; }

        public string LogMessageType { get; set; }

        public string MethodName { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }

        public string UserName { get; set; }

        public string MachineName { get; set; }

        public string Application { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}