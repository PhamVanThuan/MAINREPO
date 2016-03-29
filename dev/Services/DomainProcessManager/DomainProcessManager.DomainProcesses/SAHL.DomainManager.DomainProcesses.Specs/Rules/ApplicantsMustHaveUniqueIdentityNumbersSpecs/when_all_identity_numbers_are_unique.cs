using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Rules.ApplicantsMustHaveUniqueIdentityNumbersSpecs
{
    public class when_all_identity_numbers_are_unique : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static ApplicantsMustHaveUniqueIdentityNumbersRule rule;
        private static ApplicationCreationModel model;
        Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            model = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan, EmploymentType.Salaried);
            var applicantModel = ApplicationCreationTestHelper.PopulateApplicantModel(new List<AddressModel>());
            applicantModel.IDNumber = "1234567890123";
            var applicants = model.Applicants.ToList();
            applicants.Add(applicantModel);
            model.Applicants = applicants;
            rule = new ApplicantsMustHaveUniqueIdentityNumbersRule();
        };

        Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        It should_not_return_any_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
