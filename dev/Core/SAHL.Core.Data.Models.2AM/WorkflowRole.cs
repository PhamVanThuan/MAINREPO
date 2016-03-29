using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class WorkflowRoleDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WorkflowRoleDataModel(int legalEntityKey, int genericKey, int workflowRoleTypeKey, int generalStatusKey, DateTime statusChangeDate)
        {
            this.LegalEntityKey = legalEntityKey;
            this.GenericKey = genericKey;
            this.WorkflowRoleTypeKey = workflowRoleTypeKey;
            this.GeneralStatusKey = generalStatusKey;
            this.StatusChangeDate = statusChangeDate;
		
        }
		[JsonConstructor]
        public WorkflowRoleDataModel(int workflowRoleKey, int legalEntityKey, int genericKey, int workflowRoleTypeKey, int generalStatusKey, DateTime statusChangeDate)
        {
            this.WorkflowRoleKey = workflowRoleKey;
            this.LegalEntityKey = legalEntityKey;
            this.GenericKey = genericKey;
            this.WorkflowRoleTypeKey = workflowRoleTypeKey;
            this.GeneralStatusKey = generalStatusKey;
            this.StatusChangeDate = statusChangeDate;
		
        }		

        public int WorkflowRoleKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int GenericKey { get; set; }

        public int WorkflowRoleTypeKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public DateTime StatusChangeDate { get; set; }

        public void SetKey(int key)
        {
            this.WorkflowRoleKey =  key;
        }
    }
}