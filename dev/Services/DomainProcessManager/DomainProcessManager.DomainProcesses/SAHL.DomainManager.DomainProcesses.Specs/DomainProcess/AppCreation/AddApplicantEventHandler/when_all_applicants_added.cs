using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.AddApplicantEventHandler
{
    public class when_all_applicants_added : WithNewPurchaseDomainProcess
    {
        private static LeadApplicantAddedToApplicationEvent applicantAddedEvent;
        private static int applicationNumber;
        private static int applicationRoleKey;
        private static Dictionary<string, int> clientCollection;
        private static int clientKey;
        private static int? employerKey;
        private static ApplicantModel applicant;

        private Establish context = () =>
        {
            applicationNumber = 113234;
            applicationRoleKey = 8;
            clientKey = 119382;
            employerKey = 554897;

            var newPurchaseCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.DataModel = newPurchaseCreationModel;
            domainProcess.ProcessState = applicationStateMachine;
            applicant = newPurchaseCreationModel.Applicants.First();

            applicantAddedEvent = new LeadApplicantAddedToApplicationEvent(new DateTime(2014, 11, 15), applicationNumber, clientKey, applicationRoleKey);
            clientCollection = new Dictionary<string, int> { { applicant.IDNumber, clientKey } };
            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);
            applicationStateMachine.WhenToldTo(x => x.AllLeadApplicantsHaveBeenAdded()).Return(true);
            clientDataManager.WhenToldTo(x => x.GetEmployerKey(applicant.Employments.First().Employer.EmployerName)).Return(employerKey);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(applicantAddedEvent, serviceRequestMetadata);
        };

        private It should_fire_the_applicant_added_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed, applicantAddedEvent.Id));
        };

        private It should_delete_the_linked_key = () =>
        {
            linkedKeyManager.WasToldTo(x => x.DeleteLinkedKey(serviceRequestMetadata.CommandCorrelationId));
        };

        private It should_make_the_applicant_an_income_contributor = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(Param<MakeApplicantAnIncomeContributorCommand>.Matches(m =>
                m.ApplicationRoleKey == applicationRoleKey),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_add_client_employments = () =>
        {
            clientDomainService.WasToldTo(x => x.PerformCommand(Param.IsAny<AddUnconfirmedSalariedEmploymentToClientCommand>(),
                Param.IsAny<IServiceRequestMetadata>()));
        };

    }
}