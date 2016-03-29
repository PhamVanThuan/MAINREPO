using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ClientAddressMustBelongToAnApplicant
{
    public class when_the_client_address_does_not_belong_to_an_applicant : WithFakes
    {
        private static ClientAddressMustBelongToAnApplicantRule rule;
        private static ISystemMessageCollection messages;
        private static ApplicationMailingAddressModel model;
        private static IApplicantDataManager applicantDataManager;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            model = new ApplicationMailingAddressModel(1, 2, CorrespondenceLanguage.English, OnlineStatementFormat.PDFFormat, CorrespondenceMedium.Email, 1, true);
            applicantDataManager = An<IApplicantDataManager>();
            applicantDataManager.WhenToldTo(x => x.DoesClientAddressBelongToApplicant(model.ClientAddressKey, model.ApplicationNumber)).Return(false);
            rule = new ClientAddressMustBelongToAnApplicantRule(applicantDataManager);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_check_if_the_client_address_belongs_to_an_applicant = () =>
        {
            applicantDataManager.WasToldTo(x => x.DoesClientAddressBelongToApplicant(model.ClientAddressKey, model.ApplicationNumber));
        };

        private It should_return_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("The client address provided does not belong to an applicant.");
        };
    }
}