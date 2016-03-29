using Machine.Fakes;
using Machine.Specifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.DecisionTreeResult;
using SAHL.Services.Capitec.Managers.DecisionTreeResult.Models;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;

namespace SAHL.Services.Capitec.Specs.DecisionTreeResultsServiceSpecs
{
    public class when_getting_a_credit_assessment_tree_result : WithFakes
    {
        private static IDecisionTreeResultDataManager dataService;
        private static ILookupManager lookupService;
        private static DecisionTreeResultManager service;
        private static CreditBureauAssessmentResult result;
        private static CreditAssessmentTreeResultDataModel treeResult;
        private static ISystemMessageCollection messages;
        private static ISystemMessage message;
        private static CapitecClientCreditBureauAssessment_Query treeQuery;
        private static string treeQueryJson;
        private static string treeMessagesJson;
        private static Guid applicantID, decisionTreeResultID;
        private static JsonSerializerSettings serializerSettings;

        private Establish context = () =>
        {
            dataService = An<IDecisionTreeResultDataManager>();
            lookupService = An<ILookupManager>();
            service = new DecisionTreeResultManager(dataService, lookupService);

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

            applicantID = Guid.NewGuid();
            decisionTreeResultID = Guid.NewGuid();

            message = new SystemMessage("Credit pricing message", SystemMessageSeverityEnum.Warning);
            messages = SystemMessageCollection.Empty();
            messages.AddMessage(message);
            treeMessagesJson = JsonConvert.SerializeObject(messages, serializerSettings);

            treeQuery = new CapitecClientCreditBureauAssessment_Query(600, 0, 0, 0, 0, false, false, false, false, false, false, false, false, true);
            treeQuery.Result = new ServiceQueryResult<CapitecClientCreditBureauAssessment_QueryResult>(new List<CapitecClientCreditBureauAssessment_QueryResult> { new CapitecClientCreditBureauAssessment_QueryResult { EligibleBorrower = true } });
            treeQueryJson = JsonConvert.SerializeObject(treeQuery, serializerSettings);

            treeResult = new CreditAssessmentTreeResultDataModel(treeMessagesJson, treeQueryJson, applicantID, DateTime.Now);
            dataService.WhenToldTo(x => x.GetCreditBureauAssessmentTreeResultForApplicant(applicantID)).Return(treeResult);
        };

        private Because of = () =>
        {
            result = service.GetITCResultForApplicant(applicantID);
        };

        private It should_get_the_credit_assessment_tree_result = () =>
        {
            dataService.WasToldTo(x => x.GetCreditBureauAssessmentTreeResultForApplicant(applicantID));
        };

        private It should_return_the_parsed_messages = () =>
        {
            result.ITCMessages.AllMessages.First().Message.ShouldEqual(message.Message);
        };

        private It should_return_the_itc_passed_result = () =>
        {
            result.ITCPassed.ShouldBeTrue();
        };
    }
}