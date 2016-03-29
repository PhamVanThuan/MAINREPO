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
    public class when_getting_a_credit_pricing_tree_result : WithFakes
    {
        private static IDecisionTreeResultDataManager dataService;
        private static ILookupManager lookupService;
        private static DecisionTreeResultManager service;
        private static CreditPricingResult result;
        private static CreditPricingTreeResultDataModel treeResult;
        private static ISystemMessageCollection messages;
        private static ISystemMessage message;
        private static CapitecOriginationCreditPricing_Query treeQuery;
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
            decisionTreeResultID = Guid.NewGuid();

            message = new SystemMessage("Credit pricing message", SystemMessageSeverityEnum.Warning);
            messages = SystemMessageCollection.Empty();
            messages.AddMessage(message);
            treeMessagesJson = JsonConvert.SerializeObject(messages, serializerSettings);

            treeQuery = new CapitecOriginationCreditPricing_Query("switch", "investment", "salaried", 20000, 0, 0, 20000, 20000, 12000000, 37, 24, 240, 600, 6000, 600, 20000, true, 1500, 0, true);
            treeQuery.Result = new ServiceQueryResult<CapitecOriginationCreditPricing_QueryResult>(new List<CapitecOriginationCreditPricing_QueryResult> { new CapitecOriginationCreditPricing_QueryResult { EligibleApplication = true } });
            treeQueryJson = JsonConvert.SerializeObject(treeQuery, serializerSettings);

            treeResult = new CreditPricingTreeResultDataModel(treeMessagesJson, treeQueryJson, applicationID, DateTime.Now);
            dataService.WhenToldTo(x => x.GetCreditPricingResultForApplication(applicationID)).Return(treeResult);
        };

        private Because of = () =>
        {
            result = service.GetCalculationResultForApplication(applicationID);
        };

        private It should_get_the_credit_assessment_tree_result = () =>
        {
            dataService.WasToldTo(x => x.GetCreditPricingResultForApplication(applicationID));
        };

        private It should_return_the_messages = () =>
        {
            result.Messages.AllMessages.First().Message.ShouldEqual(message.Message);
        };

        private It should_return_the_application_eligibility = () =>
        {
            result.EligibleApplication.ShouldBeTrue();
        };
    }
}