using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class WorkflowSearchDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WorkflowSearchDataModel(int instanceID, string subject, int genericKey, string state, string workflow)
        {
            this.InstanceID = instanceID;
            this.Subject = subject;
            this.GenericKey = genericKey;
            this.State = state;
            this.Workflow = workflow;
		
        }
		[JsonConstructor]
        public WorkflowSearchDataModel(int id, int instanceID, string subject, int genericKey, string state, string workflow)
        {
            this.Id = id;
            this.InstanceID = instanceID;
            this.Subject = subject;
            this.GenericKey = genericKey;
            this.State = state;
            this.Workflow = workflow;
		
        }		

        public int Id { get; set; }

        public int InstanceID { get; set; }

        public string Subject { get; set; }

        public int GenericKey { get; set; }

        public string State { get; set; }

        public string Workflow { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}