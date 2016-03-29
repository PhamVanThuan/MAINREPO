using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Application;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Rules.OpenMortgageLoanForApplicantAndComcorpPropertyCannotExistSpecs
{
    public class when_there_is_no_existing_application : WithFakes
    {
        private static OpenMortgageLoanForApplicantAndComcorpPropertyCannotExistRule rule;
        private static IApplicationDataManager applicationDataManager;
        private static ApplicationCreationModel model;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            model = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.SwitchLoan, EmploymentType.Salaried);
            applicationDataManager = An<IApplicationDataManager>();
            applicationDataManager.WhenToldTo(x => x.DoesOpenApplicationExistForComcorpProperty(model.Applicants.First().IDNumber, model.ComcorpApplicationPropertyDetail))
                .Return(true);
            rule = new OpenMortgageLoanForApplicantAndComcorpPropertyCannotExistRule(applicationDataManager);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("An application for this property already exists against a client on this application.");
        };
    }
}