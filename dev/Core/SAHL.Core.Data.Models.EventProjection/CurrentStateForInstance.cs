using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.EventProjection
{
    [Serializable]
    public partial class CurrentStateForInstanceDataModel :  IDataModel, IDataModelWithIdentitySeed, IDataModelWithPrimaryKeyId 
    {
        public CurrentStateForInstanceDataModel(DateTime stateChangeDate, long instanceId, string stateName, string workflowName, int genericKeyTypeKey, int genericKey, int? daysInState)
        {
            this.StateChangeDate = stateChangeDate;
            this.InstanceId = instanceId;
            this.StateName = stateName;
            this.WorkflowName = workflowName;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.DaysInState = daysInState;
		
        }
		[JsonConstructor]
        public CurrentStateForInstanceDataModel(int id, DateTime stateChangeDate, long instanceId, string stateName, string workflowName, int genericKeyTypeKey, int genericKey, int? daysInState)
        {
            this.Id = id;
            this.StateChangeDate = stateChangeDate;
            this.InstanceId = instanceId;
            this.StateName = stateName;
            this.WorkflowName = workflowName;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.DaysInState = daysInState;
		
        }		

        public int Id { get; set; }

        public DateTime StateChangeDate { get; set; }

        public long InstanceId { get; set; }

        public string StateName { get; set; }

        public string WorkflowName { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int GenericKey { get; set; }

        public int? DaysInState { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}