using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Rules.ApplicationMustHaveAtLeastOneMainApplicantSpecs
{
    public class when_there_are_no_main_applicants : WithFakes
    {
        private static ApplicationMustHaveAtLeastOneMainApplicantRule rule;
        private static ApplicationCreationModel model;
        private static ISystemMessageCollection systemMessageCollection;

        private Establish context = () =>
        {
            systemMessageCollection = SystemMessageCollection.Empty();
            model = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan, EmploymentType.Salaried);
            foreach (var applicant in model.Applicants)
            {
                applicant.ApplicantRoleType = LeadApplicantOfferRoleTypeEnum.Lead_Suretor;
            }
            rule = new ApplicationMustHaveAtLeastOneMainApplicantRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(systemMessageCollection, model);
        };

        private It should_return_no_messages = () =>
        {
            systemMessageCollection.ErrorMessages().First().Message.ShouldEqual("An application must have at least one main applicant.");
        };
    }
}