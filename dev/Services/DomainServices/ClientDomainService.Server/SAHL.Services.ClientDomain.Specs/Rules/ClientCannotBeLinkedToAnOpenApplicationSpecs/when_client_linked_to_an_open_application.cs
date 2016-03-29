using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.ClientDomain.Rules.Models;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.ClientCannotBeLinkedToAnOpenApplicationSpecs
{
    public class when_client_linked_to_an_open_application : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static ClientCannotBeLinkedToAnOpenApplicationRule rule;
        private static NaturalPersonClientRuleModel ruleModel;
        private static IClientDataManager clientDataManager;
        private static int clientKey = 1234;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            clientDataManager = An<IClientDataManager>();
            rule = new ClientCannotBeLinkedToAnOpenApplicationRule(clientDataManager);
            ruleModel = new NaturalPersonClientRuleModel(clientKey, "1234566778", "sdfdsfds", null, "FirstName", "LastName", "I", "", 
                                                         null, null, null, null, null, null, null, null, "", "", "", "", "", "", "", "");

            // set the clientdatamanager to return that the client IS linked to an open account
            clientDataManager.WhenToldTo(x => x.FindOpenApplicationNumbersForClient(clientKey))
                .Return((new int[] { 11111 }));
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_return_error_messages = () =>
        {
            messages.ErrorMessages().Any().ShouldEqual(true);
        };
    }
}