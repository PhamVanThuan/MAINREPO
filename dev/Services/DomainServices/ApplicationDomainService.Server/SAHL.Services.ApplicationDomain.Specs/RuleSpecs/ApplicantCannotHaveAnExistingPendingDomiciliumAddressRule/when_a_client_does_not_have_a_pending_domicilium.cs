using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Domicilium;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ApplicantCannotHaveAnExistingPendingDomiciliumAddress
{
    public class when_a_client_does_not_have_a_pending_domicilium : WithFakes
    {
        static IApplicantDataManager applicantDataManager;
        static ApplicantDomiciliumAddressModel ruleModel;
        static int clientDomiciliumKey, applicationNumber, clientKey;
        static ISystemMessageCollection systemMessageCollection;
        static ApplicantCannotHaveAnExistingPendingDomiciliumAddressRule rule;

        Establish context = () =>
        {
            clientDomiciliumKey = 100;
            applicationNumber = 123;
            clientKey = 9090;
            systemMessageCollection = SystemMessageCollection.Empty();
            applicantDataManager = An<IApplicantDataManager>();
            ruleModel = new ApplicantDomiciliumAddressModel(clientDomiciliumKey, applicationNumber, clientKey);
            rule = new ApplicantCannotHaveAnExistingPendingDomiciliumAddressRule(applicantDataManager);

            applicantDataManager.WhenToldTo(x => x.DoesApplicantHavePendingDomiciliumOnApplication(ruleModel.ClientKey, ruleModel.ApplicationNumber)).Return(false);

        };

        Because of = () =>
        {
            rule.ExecuteRule(systemMessageCollection, ruleModel);
        };

        It should_check_if_client_has_pending_domicilium = () =>
        {
            applicantDataManager.WasToldTo(x => x.DoesApplicantHavePendingDomiciliumOnApplication(ruleModel.ClientKey, ruleModel.ApplicationNumber));
        };

        It should_not_contain_error_messages = () =>
        {
            systemMessageCollection.ErrorMessages().Count().ShouldEqual(0);
        };
    }
}
