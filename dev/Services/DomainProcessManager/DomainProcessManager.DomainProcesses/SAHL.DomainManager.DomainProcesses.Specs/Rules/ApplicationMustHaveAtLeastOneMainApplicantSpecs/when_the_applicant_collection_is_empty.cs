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
    public class when_the_applicant_collection_is_empty : WithFakes
    {
        private static ApplicationMustHaveAtLeastOneMainApplicantRule rule;
        private static ApplicationCreationModel model;
        private static ISystemMessageCollection systemMessageCollection;

        private Establish context = () =>
        {
            systemMessageCollection = SystemMessageCollection.Empty();
            model = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan, EmploymentType.Salaried);
            model.Applicants = new List<ApplicantModel>();
            rule = new ApplicationMustHaveAtLeastOneMainApplicantRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(systemMessageCollection, model);
        };

        private It should_return_a_message = () =>
        {
            systemMessageCollection.ErrorMessages().First().Message.ShouldEqual("An application must have at least one main applicant.");
        };
    }
}