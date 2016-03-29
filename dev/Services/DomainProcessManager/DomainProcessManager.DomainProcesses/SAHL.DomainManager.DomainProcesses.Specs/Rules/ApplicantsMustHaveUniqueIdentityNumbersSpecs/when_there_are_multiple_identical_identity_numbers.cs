using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;



namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Rules.ApplicantsMustHaveUniqueIdentityNumbersSpecs
{
    public class when_there_are_multiple_identical_identity_numbers : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static ApplicantsMustHaveUniqueIdentityNumbersRule rule;
        private static ApplicationCreationModel model;
        Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            model = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan, EmploymentType.Salaried);

            var applicantModel1 = ApplicationCreationTestHelper.PopulateApplicantModel(new List<AddressModel>());
            applicantModel1.IDNumber = "1234567890123";

            var applicantModel2 = ApplicationCreationTestHelper.PopulateApplicantModel(new List<AddressModel>());
            applicantModel2.IDNumber = "1234567890123";

            var applicants = model.Applicants.ToList();
            applicants.Add(applicantModel2);
            applicants.Add(applicantModel1);
            model.Applicants = applicants;

            rule = new ApplicantsMustHaveUniqueIdentityNumbersRule();
        };

        Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        It should_return_a_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("The identity number for each applicant must be unique.");
        };
    }
}
