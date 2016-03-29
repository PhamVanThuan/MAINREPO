using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.NewBusinessMortgageLoanApplication
{
    public class when_handling__event : WithNewPurchaseDomainProcess
    {
        private static LeadApplicantAddedToApplicationEvent applicantAddedToNewBusinessMortgageLoanApplicationEvent;
        private static int applicationRoleKey;
        private static int applicationNumber;
        private static int clientKey;

        private Establish context = () =>
        {
            domainProcess.ProcessState = applicationStateMachine;
            var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.DataModel = applicationCreationModel;

            applicationNumber = 173;
            applicationRoleKey = 568;
            clientKey = 3071;

            applicantAddedToNewBusinessMortgageLoanApplicationEvent = new LeadApplicantAddedToApplicationEvent(new DateTime(2014, 9, 25), applicationNumber, clientKey, applicationRoleKey);
            var clientCollection = new Dictionary<string, int>();
            clientCollection.Add(domainProcess.DataModel.Applicants.First().IDNumber, clientKey);

            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(applicantAddedToNewBusinessMortgageLoanApplicationEvent, serviceRequestMetadata);
        };

        private It should_save_the_income_contributor = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(
                      Param<MakeApplicantAnIncomeContributorCommand>.Matches(y => y.ApplicationRoleKey == applicantAddedToNewBusinessMortgageLoanApplicationEvent.ApplicationRoleKey)
                    , Param.IsAny<IServiceRequestMetadata>()
                ));
        };

        private It should_delete_the_application_role_key_from_the_linked_key_manager = () =>
        {
            linkedKeyManager.WasToldTo(x => x.DeleteLinkedKey(Param<Guid>.Matches(y => y == serviceRequestMetadata.CommandCorrelationId)));
        };

        private It should_fire_applicant_added_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed, applicantAddedToNewBusinessMortgageLoanApplicationEvent.Id));
        };
    }
}