using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Domicilium;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.IsDomiciliumAddressPendingDomiciliumAddress
{
    public class when_domicilium_address_is_a_pending_domicilium_address : WithFakes
    {
        private static IDomiciliumDataManager domiciliumDataManager;
        private static ApplicantDomiciliumAddressModel ruleModel;
        private static int clientDomiciliumKey, applicationNumber, clientKey;
        private static ISystemMessageCollection systemMessageCollection;
        private static ApplicantCanOnlyBeLinkedToAPendingDomiciliumAddressRule rule;

        private Establish context = () =>
        {
            clientDomiciliumKey = 100;
            applicationNumber = 123;
            clientKey = 9090;
            systemMessageCollection = SystemMessageCollection.Empty();
            domiciliumDataManager = An<IDomiciliumDataManager>();
            ruleModel = new ApplicantDomiciliumAddressModel(clientDomiciliumKey, applicationNumber, clientKey);
            rule = new ApplicantCanOnlyBeLinkedToAPendingDomiciliumAddressRule(domiciliumDataManager);
            domiciliumDataManager.WhenToldTo(x => x.IsDomiciliumAddressPendingDomiciliumAddress(clientDomiciliumKey)).Return(true);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(systemMessageCollection, ruleModel);
        };

        private It should_check_if_client_has_pending_domicilium = () =>
        {
            domiciliumDataManager.WasToldTo(x => x.IsDomiciliumAddressPendingDomiciliumAddress(ruleModel.ClientDomiciliumKey));
        };

        private It should_not_contain_error_messages = () =>
        {
            systemMessageCollection.ErrorMessages().Count().ShouldEqual(0);
        };
    }
}