using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.DecisionTreeResult.Models;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;

namespace SAHL.Services.Capitec.Managers.DecisionTreeResult
{
    public class DecisionTreeResultManager : IDecisionTreeResultManager
    {
        private IDecisionTreeResultDataManager dataService;
        private ILookupManager lookupService;
        private JsonSerializerSettings serializerSettings;

        public DecisionTreeResultManager(IDecisionTreeResultDataManager dataService, ILookupManager lookupService)
        {
            this.dataService = dataService;
            this.lookupService = lookupService;
            serializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Full,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                ContractResolver = new DefaultContractResolver
                {
                    DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.Instance
                }
            };
        }

        public Guid SaveCreditPricingTreeResult(object treeQuery, ISystemMessageCollection decisionTreeMessages, Guid applicationID)
        {
            var treeQueryJson = JsonConvert.SerializeObject(treeQuery, serializerSettings);
            var decisionTreeMessagesJson = JsonConvert.SerializeObject(decisionTreeMessages, serializerSettings);
            var decisionTreeResultID = lookupService.GenerateCombGuid();

            dataService.SaveCreditPricingTreeResult(decisionTreeResultID, treeQueryJson, decisionTreeMessagesJson, applicationID, DateTime.Now);

            return decisionTreeResultID;
        }

        public Guid SaveCreditAssessmentTreeResult(object treeQuery, ISystemMessageCollection decisionTreeMessages, Guid applicantID)
        {
            var decisionTreeResultID = lookupService.GenerateCombGuid();

            var treeQueryJson = JsonConvert.SerializeObject(treeQuery, serializerSettings);
            var decisionTreeMessagesJson = JsonConvert.SerializeObject(decisionTreeMessages, serializerSettings);

            dataService.SaveCreditAssessmentTreeResult(decisionTreeResultID, treeQueryJson, decisionTreeMessagesJson, applicantID, DateTime.Now);

            return decisionTreeResultID;
        }

        public CreditBureauAssessmentResult GetITCResultForApplicant(Guid applicantID)
        {
            CreditBureauAssessmentResult result = new CreditBureauAssessmentResult();

            var decisionTreeResult = dataService.GetCreditBureauAssessmentTreeResultForApplicant(applicantID);
            if (decisionTreeResult != null)
            {
                result.ITCMessages = JsonConvert.DeserializeObject<SystemMessageCollection>(decisionTreeResult.Messages, serializerSettings);
                var treeQuery = JsonConvert.DeserializeObject<CapitecClientCreditBureauAssessment_Query>(decisionTreeResult.TreeQuery, serializerSettings);
                result.ITCPassed = treeQuery.Result.Results.First().EligibleBorrower;
            }
            else
            {
                result.ITCPassed = true;
                result.ITCMessages = new SystemMessageCollection();
                result.ITCMessages.AddMessage(new SystemMessage("No credit bureau check was performed.", SystemMessageSeverityEnum.Warning));
            }

            return result;
        }

        public CreditPricingResult GetCalculationResultForApplication(Guid applicationID)
        {
            CreditPricingResult result = new CreditPricingResult();

            var decisionTreeResult = dataService.GetCreditPricingResultForApplication(applicationID);
            if (decisionTreeResult != null)
            {
                result.Messages = JsonConvert.DeserializeObject<SystemMessageCollection>(decisionTreeResult.Messages, serializerSettings);
                var treeQuery = JsonConvert.DeserializeObject<CapitecOriginationCreditPricing_Query>(decisionTreeResult.TreeQuery, serializerSettings);
                result.EligibleApplication = treeQuery.Result.Results.First().EligibleApplication;
            }
            return result;
        }
    }
}