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
    public class when_applicants_are_empty : WithFakes
    {
        private static ApplicantsMustHaveUniqueIdentityNumbersRule rule;
        private static ApplicationCreationModel model;
        private static ISystemMessageCollection messages;

        Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            model = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan, EmploymentType.Salaried);
            model.Applicants = new List<ApplicantModel>();
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
