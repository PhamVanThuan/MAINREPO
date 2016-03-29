using Automation.DataAccess.DataHelper;
using Automation.DataModels;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class LifeService : _2AMDataHelper, ILifeService
    {
        private IX2WorkflowService x2WorkflowService;
        public LifeService(IX2WorkflowService x2WorkflowService)
        {
            this.x2WorkflowService = x2WorkflowService;
        }
        public void CreateInstance(int mortgageLoanNumber)
        {
            var activityXMLData = String.Format(@"'<FieldInputs><FieldInput Name = ""LoanNumber"">{0}</FieldInput><FieldInput Name = ""AssignTo"">Token</FieldInput></FieldInputs>'", mortgageLoanNumber);
            this.x2WorkflowService.InsertActiveExternalActivity(Workflows.LifeOrigination, ExternalActivities.Life.CreateInstance, -1, -1, activityXMLData: activityXMLData);
        
        }
        public IEnumerable<LifeLead> GetLifeLeads(string workflowState, string consultantName, LifePolicyTypeEnum lifePolicyTypeEnum)
        {
            return base.GetLifeLeads(0,workflowState,WorkflowStates.LifeOriginationWF.AwaitingTimeout,lifePolicyTypeEnum)
                 .Where(x => x.AssignedConsultant == consultantName);
        }
        public IEnumerable<LifeLead> GetLifeLeads(int mortgageAccountKey)
        {
            return base.GetLifeLeads(mortgageAccountKey, null, WorkflowStates.LifeOriginationWF.AwaitingTimeout, LifePolicyTypeEnum.StandardCover)
                 .Where(x=>x.AccountKey == mortgageAccountKey);
        }

        public IEnumerable<LifeLead> GetLifeLeads(string workflowState, int mortgageAccountKey, LifePolicyTypeEnum lifePolicyTypeEnum)
        {
            return base.GetLifeLeads(mortgageAccountKey, workflowState, WorkflowStates.LifeOriginationWF.AwaitingTimeout, lifePolicyTypeEnum);
        }
       
        public IEnumerable<string> GetLifeWorkflowStateNames()
        {
            var workflow = base.GetWorkflows()
                .Where(x => x.Name == "LifeOrigination").FirstOrDefault();
            return base.GetStates(workflow).Select(x => x.Name);
        }
    }
}