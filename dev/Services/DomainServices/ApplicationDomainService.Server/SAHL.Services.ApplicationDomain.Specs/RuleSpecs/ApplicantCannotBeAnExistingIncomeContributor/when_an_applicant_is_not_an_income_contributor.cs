using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ApplicantCannotBeAnExistingIncomeContributor
{
    public class when_an_applicant_is_not_an_income_contributor : WithCoreFakes
    {
        private static ApplicantCannotBeAnExistingIncomeContributorRule rule;
        private static ApplicantRoleModel applicantRoleModel;
        private static IApplicantManager applicantManager;

        private Establish context = () =>
            {
                applicantRoleModel = new ApplicantRoleModel(25868);
                applicantManager = An<IApplicantManager>();
                rule = new ApplicantCannotBeAnExistingIncomeContributorRule(applicantManager);
                applicantManager.WhenToldTo(x => x.IsApplicantAnIncomeContributor(Param.IsAny<int>())).Return(false);
            };

        private Because of = () =>
            {
                rule.ExecuteRule(messages, applicantRoleModel);
            };

        private It should_not_return_a_message = () =>
            {
                messages.ErrorMessages().ShouldBeEmpty();
            };
    };
}