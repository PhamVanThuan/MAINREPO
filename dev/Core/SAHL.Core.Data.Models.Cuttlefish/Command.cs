using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class CommandDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CommandDataModel(string data, DateTime commandInsertDate, string machineName, string serviceName, bool hasCompleted, bool hasFailed, string contextValues, bool notAuthenticated, bool notAuthorized)
        {
            this.Data = data;
            this.CommandInsertDate = commandInsertDate;
            this.MachineName = machineName;
            this.ServiceName = serviceName;
            this.HasCompleted = hasCompleted;
            this.HasFailed = hasFailed;
            this.ContextValues = contextValues;
            this.NotAuthenticated = notAuthenticated;
            this.NotAuthorized = notAuthorized;
		
        }

        public CommandDataModel(int commandKey, string data, DateTime commandInsertDate, string machineName, string serviceName, bool hasCompleted, bool hasFailed, string contextValues, bool notAuthenticated, bool notAuthorized)
        {
            this.CommandKey = commandKey;
            this.Data = data;
            this.CommandInsertDate = commandInsertDate;
            this.MachineName = machineName;
            this.ServiceName = serviceName;
            this.HasCompleted = hasCompleted;
            this.HasFailed = hasFailed;
            this.ContextValues = contextValues;
            this.NotAuthenticated = notAuthenticated;
            this.NotAuthorized = notAuthorized;
		
        }		

        public int CommandKey { get; set; }

        public string Data { get; set; }

        public DateTime CommandInsertDate { get; set; }

        public string MachineName { get; set; }

        public string ServiceName { get; set; }

        public bool HasCompleted { get; set; }

        public bool HasFailed { get; set; }

        public string ContextValues { get; set; }

        public bool NotAuthenticated { get; set; }

        public bool NotAuthorized { get; set; }

        public void SetKey(int key)
        {
            this.CommandKey =  key;
        }
    }
}