using Machine.Fakes;
using Machine.Specifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.DecisionTreeResult;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;

namespace SAHL.Services.Capitec.Specs.DecisionTreeResultsServiceSpecs
{
    public class when_saving_a_credit_assessment_tree_result : WithFakes
    {
        private static IDecisionTreeResultDataManager dataService;
        private static ILookupManager lookupService;
        private static DecisionTreeResultManager service;
        private static CapitecClientCreditBureauAssessment_Query treeQuery;
        private static ISystemMessageCollection messages;
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
            messages = SystemMessageCollection.Empty();
            treeQuery = new CapitecClientCreditBureauAssessment_Query(600, 0, 0, 0, 0, false, false, false, false, false, false, false, false, true);
            treeQueryJson = JsonConvert.SerializeObject(treeQuery, serializerSettings);
            treeMessagesJson = JsonConvert.SerializeObject(messages, serializerSettings);
            decisionTreeResultID = Guid.NewGuid();
            lookupService.WhenToldTo(x => x.GenerateCombGuid()).Return(decisionTreeResultID);
        };

        private Because of = () =>
        {
            service.SaveCreditAssessmentTreeResult(treeQuery, messages, applicantID);
        };

        private It should_get_an_id_for_the_result = () =>
        {
            lookupService.WasToldTo(x => x.GenerateCombGuid());
        };

        private It should_save_the_credit_pricing_tree_result = () =>
        {
            dataService.WasToldTo(x => x.SaveCreditAssessmentTreeResult(decisionTreeResultID, treeQueryJson, treeMessagesJson, applicantID, Param.IsAny<DateTime>()));
        };
    }
}