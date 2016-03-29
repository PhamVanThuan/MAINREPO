using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Rules.ApplicationMustHaveAtLeastOneMainApplicantSpecs
{
    public class when_there_is_a_main_applicant : WithFakes
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
                applicant.ApplicantRoleType = LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant;
            }
            rule = new ApplicationMustHaveAtLeastOneMainApplicantRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(systemMessageCollection, model);
        };

        private It should_return_no_messages = () =>
        {
            systemMessageCollection.AllMessages.ShouldBeEmpty();
        };
    }
}