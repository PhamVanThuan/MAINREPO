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
    public class when_saving_a_credit_pricing_tree_result : WithFakes
    {
        private static IDecisionTreeResultDataManager dataService;
        private static ILookupManager lookupService;
        private static DecisionTreeResultManager service;
        private static CapitecOriginationCreditPricing_Query treeQuery;
        private static ISystemMessageCollection messages;
        private static string treeQueryJson;
        private static string treeMessagesJson;
        private static Guid applicationID, decisionTreeResultID;
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
            applicationID = Guid.NewGuid();
            messages = SystemMessageCollection.Empty();
            treeQuery = new CapitecOriginationCreditPricing_Query("switch", "investment", "salaried", 20000, 0, 0, 20000, 20000, 12000000, 37, 24, 240, 600, 6000, 600, 20000, true, 1500, 0, true);
            treeQueryJson = JsonConvert.SerializeObject(treeQuery, serializerSettings);
            treeMessagesJson = JsonConvert.SerializeObject(messages, serializerSettings);
            decisionTreeResultID = Guid.NewGuid();
            lookupService.WhenToldTo(x => x.GenerateCombGuid()).Return(decisionTreeResultID);
        };

        private Because of = () =>
        {
            service.SaveCreditPricingTreeResult(treeQuery, messages, applicationID);
        };

        private It should_get_an_id_for_the_result = () =>
        {
            lookupService.WasToldTo(x => x.GenerateCombGuid());
        };

        private It should_save_the_credit_pricing_tree_result = () =>
        {
            dataService.WasToldTo(x => x.SaveCreditPricingTreeResult(decisionTreeResultID, treeQueryJson, treeMessagesJson, applicationID, Param.IsAny<DateTime>()));
        };
    }
}