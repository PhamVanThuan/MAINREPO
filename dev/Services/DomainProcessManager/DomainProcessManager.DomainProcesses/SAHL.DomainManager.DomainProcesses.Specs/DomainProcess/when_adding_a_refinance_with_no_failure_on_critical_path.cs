using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Behaviors;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.Services.Interfaces.FinancialDomain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess
{
    public class when_adding_a_refinance_with_no_failure_on_critical_path : WithDomainServiceMocks
    {
        protected static RefinanceApplicationDomainProcess domainProcess;
        protected static RefinanceApplicationCreationModel refinanceCreationModel;

        protected static ApplicantModel applicant;

        private static RefinanceApplicationAddedEvent RefinanceApplicationAddedEvent;
        private static NaturalPersonClientAddedEvent naturalPersonClientAddedEvent;
        private static LeadApplicantAddedToApplicationEvent applicantAddedEvent;
        private static EmployerAddedEvent employerAddedEvent;
        private static UnconfirmedSalariedEmploymentAddedToClientEvent unconfirmedSalaryAddedEvent;
        private static ApplicationHouseholdIncomeDeterminedEvent householdIncomeDeterminedEvent;
        private static ApplicationEmploymentTypeSetEvent applicationEmploymentSetEvent;
        private static NewBusinessApplicationPricedEvent newBusinessPricedEvent;
        private static NewBusinessApplicationFundedEvent newBusinessFundedEvent;

        protected static int applicationNumber, clientKey, applicationRoleKey, employerKey, employmentKey, employmentTypeKey;
        protected static double householdIncome;
        protected static ServiceRequestMetadata metadata;
        private static IRawLogger rawLogger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;

        private Establish context = () =>
        {
            rawLogger = An<IRawLogger>();
            loggerSource = An<ILoggerSource>();
            loggerAppSource = An<ILoggerAppSource>();
            applicationStateMachine = new ApplicationStateMachine();

            domainProcess = new RefinanceApplicationDomainProcess(
                                      applicationStateMachine
                                    , applicationDomainService
                                    , clientDomainService
                                    , addressDomainService
                                    , financialDomainService
                                    , bankAccountDomainService
                                    , combGuidGenerator
                                    , clientDataManager
                                    , x2WorkflowManager
                                    , linkedKeyManager
                                    , propertyDomainService
                                    , communicationManager
                                    , applicationDataManager
                                    , domainRuleManager
                                    , rawLogger
                                    , loggerSource
                                    , loggerAppSource
                               );
            CurrentlyPerformingRequest += domainProcess.StoreCurrentlyPerformingRequestCounter;
            sentCommands.Clear();
            refinanceCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.RefinanceLoan) as RefinanceApplicationCreationModel;

            applicationRoleKey = 5;
            applicationNumber = 15006;
            employmentKey = 90579;
            employmentTypeKey = 30;
            clientKey = 554;
            employerKey = 95;
            householdIncome = 60000;

            var testDate = new DateTime(2014, 10, 11);
            RefinanceApplicationAddedEvent = DomainProcessTestHelper.GetRefinanceApplicationAddedEvent(applicationNumber, refinanceCreationModel, testDate);

            applicant = refinanceCreationModel.Applicants.First();
            naturalPersonClientAddedEvent = DomainProcessTestHelper.GetNaturalPersonClientAddedEvent(clientKey, applicant, testDate);
            applicantAddedEvent = DomainProcessTestHelper.GetLeadApplicantAddedEvent(applicationNumber, clientKey, applicationRoleKey, testDate);

            var employer = applicant.Employments.First().Employer;
            employerAddedEvent = DomainProcessTestHelper.GetEmployerAddedEvent(employerKey, employer, testDate);
            unconfirmedSalaryAddedEvent = DomainProcessTestHelper.GetSalariedEmploymentAddedEvent(clientKey, employmentKey, applicant.Employments.First() as Models.SalariedEmploymentModel, testDate);
            applicationEmploymentSetEvent = DomainProcessTestHelper.GetApplicationEmploymentTypeSetEvent(applicationNumber, employmentTypeKey, testDate);
            householdIncomeDeterminedEvent = DomainProcessTestHelper.GetApplicationHouseholdIncomeDeterminedEvent(applicationNumber, householdIncome, testDate);

            newBusinessPricedEvent = DomainProcessTestHelper.GetApplicationPricedEvent(applicationNumber, testDate);
            newBusinessFundedEvent = DomainProcessTestHelper.GetApplicationFundedEvent(applicationNumber, testDate);

            applicationEmploymentSetEvent = new ApplicationEmploymentTypeSetEvent(new DateTime(2014, 10, 9), applicationNumber, employmentTypeKey);
            householdIncomeDeterminedEvent = new ApplicationHouseholdIncomeDeterminedEvent(new DateTime(2014, 10, 9), applicationNumber, householdIncome);

            newBusinessPricedEvent = new NewBusinessApplicationPricedEvent(new DateTime(2014, 10, 9), applicationNumber);
            newBusinessFundedEvent = new NewBusinessApplicationFundedEvent(new DateTime(2014, 10, 9), applicationNumber);

            clientDomainService.WhenToldTo(x => x.PerformQuery(Param.IsAny<FindClientByIdNumberQuery>())).Return<FindClientByIdNumberQuery>(y =>
            {
                y.Result = new ServiceQueryResult<ClientDetailsQueryResult>(new List<ClientDetailsQueryResult>() { null });
                return new SystemMessageCollection();
            });

            clientDomainService.WhenToldTo(x => x.PerformQuery(Param.IsAny<FindClientByPassportNumberQuery>())).Return<FindClientByPassportNumberQuery>(y =>
            {
                y.Result = new ServiceQueryResult<ClientDetailsQueryResult>(new List<ClientDetailsQueryResult> { null });
                return new SystemMessageCollection();
            });
        };

        private Because of = () =>
        {
            domainProcess.Start(refinanceCreationModel, typeof(RefinanceApplicationAddedEvent).Name);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, Guid.NewGuid());
            domainProcess.HandleEvent(RefinanceApplicationAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, Guid.NewGuid()) { { "IdNumberOfAddedClient", applicant.IDNumber } };
            domainProcess.HandleEvent(naturalPersonClientAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, Guid.NewGuid());
            domainProcess.HandleEvent(applicantAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, Guid.NewGuid()) { { "EmployeeIdNumber", applicant.IDNumber } };
            domainProcess.HandleEvent(employerAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, Guid.NewGuid());
            domainProcess.HandleEvent(unconfirmedSalaryAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, Guid.NewGuid());
            domainProcess.HandleEvent(householdIncomeDeterminedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, Guid.NewGuid());
            domainProcess.HandleEvent(applicationEmploymentSetEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, Guid.NewGuid());
            domainProcess.Handle(newBusinessPricedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, Guid.NewGuid());
            domainProcess.HandleEvent(newBusinessFundedEvent, metadata);
        };

        private It should_add_a_refinance_application = () =>
        {
            applicationDomainService.WasToldTo(ads => ads.PerformCommand(
                Param<AddRefinanceApplicationCommand>.Matches(m =>
                m.RefinanceApplicationModel.ApplicationType == refinanceCreationModel.ApplicationType &&
                m.RefinanceApplicationModel.EstimatedPropertyValue == refinanceCreationModel.EstimatedPropertyValue &&
                m.RefinanceApplicationModel.CashOut == refinanceCreationModel.CashOut &&
                m.RefinanceApplicationModel.OriginationSource == refinanceCreationModel.OriginationSource &&
                m.RefinanceApplicationModel.Product == refinanceCreationModel.Product),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private Behaves_like<DomainProcessThatHasAddedANewApplicant> a_domain_process_that_has_added_a_new_applicant = () => { };
        private Behaves_like<DomainProcessThatHasAddedApplicationDetails> a_new_purchase_process_that_has_added_the_application_details = () => { };

        private It should_not_be_complete = () =>
        {
            applicationStateMachine.HasProcessCompletedWithCriticalPathFullyCaptured().ShouldBeFalse();
            applicationStateMachine.ContainsStateInBreadCrumb(ApplicationState.Completed).ShouldBeFalse();
        };
    }
}