﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
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

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.NewBusinessMortgageLoanApplication
{
    public class when_adding_employment_and_no_employer_exists : WithNewPurchaseDomainProcess
    {
        private static LeadApplicantAddedToApplicationEvent applicantAddedToNewBusinessMortgageLoanApplicationEvent;
        private static int applicationRoleKey;
        private static int applicationNumber;
        private static int clientKey;
        private static EmployerModel employer;

        private Establish context = () =>
        {
            domainProcess.ProcessState = applicationStateMachine;
            var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.DataModel = applicationCreationModel;
            employer = applicationCreationModel.Applicants.First().Employments.First().Employer;

            applicationNumber = 173;
            applicationRoleKey = 568;
            clientKey = 3071;

            applicantAddedToNewBusinessMortgageLoanApplicationEvent = new LeadApplicantAddedToApplicationEvent(new DateTime(2014, 9, 25), applicationNumber, clientKey, applicationRoleKey);
            var clientCollection = new Dictionary<string, int>();
            clientCollection.Add(domainProcess.DataModel.Applicants.First().IDNumber, clientKey);

            applicationStateMachine.WhenToldTo(x => x.AllLeadApplicantsHaveBeenAdded()).Return(true);
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
                    , Param.IsAny<DomainProcessServiceRequestMetadata>()
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

        private It should_add_the_employer = () =>
        {
            clientDomainService.WasToldTo(x => x.PerformCommand(Param<AddEmployerCommand>.Matches(m =>
                m.Employer.EmployerName == employer.EmployerName &&
                m.Employer.EmployerBusinessType == employer.EmployerBusinessType),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };
    }
}