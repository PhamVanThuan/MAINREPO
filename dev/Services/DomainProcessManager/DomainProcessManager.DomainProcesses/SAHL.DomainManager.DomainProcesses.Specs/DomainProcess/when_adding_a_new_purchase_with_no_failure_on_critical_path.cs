using System;
using System.Collections.Generic;
using System.Linq;
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
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using SAHL.Services.Interfaces.FinancialDomain.Events;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess
{
    public class when_adding_a_new_purchase_with_no_failure_on_critical_path : WithDomainServiceMocks
    {
        protected static NewPurchaseApplicationDomainProcess domainProcess;
        protected static NewPurchaseApplicationCreationModel newPurchaseCreationModel;

        protected static ApplicantModel applicant;

        private static NewPurchaseApplicationAddedEvent newPurchaseApplicationAddedEvent;
        private static NaturalPersonClientAddedEvent naturalPersonClientAddedEvent;
        private static LeadApplicantAddedToApplicationEvent applicantAddedEvent;
        private static EmployerAddedEvent employerAddedEvent;
        private static UnconfirmedSalariedEmploymentAddedToClientEvent unconfirmedSalaryAddedEvent;
        private static ApplicationHouseholdIncomeDeterminedEvent householdIncomeDeterminedEvent;
        private static ApplicationEmploymentTypeSetEvent applicationEmploymentSetEvent;
        private static NewBusinessApplicationPricedEvent newBusinessPricedEvent;
        private static NewBusinessApplicationFundedEvent newBusinessFundedEvent;
        private static IRawLogger rawLogger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;

        protected static int applicationNumber, clientKey, applicationRoleKey, employerKey, employmentKey, employmentTypeKey;
        private static double householdIncome;
        protected static DomainProcessServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            rawLogger = An<IRawLogger>();
            loggerSource = An<ILoggerSource>();
            loggerAppSource = An<ILoggerAppSource>();
            applicationStateMachine = new ApplicationStateMachine();

            domainProcess = new NewPurchaseApplicationDomainProcess(
                applicationStateMachine,
                applicationDomainService,
                clientDomainService,
                addressDomainService,
                financialDomainService,
                bankAccountDomainService,
                combGuidGenerator,
                clientDataManager,
                x2WorkflowManager,
                linkedKeyManager,
                propertyDomainService,
                communicationManager,
                applicationDataManager,
                domainRuleManager,
                rawLogger,
                loggerSource,
                loggerAppSource);

            CurrentlyPerformingRequest += domainProcess.StoreCurrentlyPerformingRequestCounter;
            sentCommands.Clear();
            newPurchaseCreationModel =
                ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            applicationRoleKey = 5;
            applicationNumber = 15006;
            employmentKey = 79567;
            employmentTypeKey = 30;
            clientKey = 554;
            employerKey = 95;
            householdIncome = 60000;

            applicationDataManager.WhenToldTo(adm => adm.DoesSuppliedVendorExist(Param.IsAny<string>())).Return(true);

            var testDate = new DateTime(2014, 10, 9);
            newPurchaseApplicationAddedEvent = DomainProcessTestHelper.GetNewPurchaseApplicationAddedEvent(applicationNumber,
                newPurchaseCreationModel,
                testDate);

            applicant = newPurchaseCreationModel.Applicants.First();
            naturalPersonClientAddedEvent = DomainProcessTestHelper.GetNaturalPersonClientAddedEvent(clientKey, applicant, testDate);
            applicantAddedEvent = DomainProcessTestHelper.GetLeadApplicantAddedEvent(applicationNumber, clientKey, applicationRoleKey, testDate);

            employerAddedEvent = DomainProcessTestHelper.GetEmployerAddedEvent(employerKey, applicant.Employments.First().Employer, testDate);

            var salariedEmployment = applicant.Employments.First() as SalariedEmploymentModel;
            unconfirmedSalaryAddedEvent = DomainProcessTestHelper.GetSalariedEmploymentAddedEvent(clientKey,
                employmentKey,
                salariedEmployment,
                testDate);

            applicationEmploymentSetEvent = DomainProcessTestHelper.GetApplicationEmploymentTypeSetEvent(applicationNumber,
                employmentTypeKey,
                testDate);
            householdIncomeDeterminedEvent = DomainProcessTestHelper.GetApplicationHouseholdIncomeDeterminedEvent(applicationNumber,
                householdIncome,
                testDate);

            newBusinessPricedEvent = new NewBusinessApplicationPricedEvent(new DateTime(2014, 10, 9), applicationNumber);
            newBusinessFundedEvent = new NewBusinessApplicationFundedEvent(new DateTime(2014, 10, 9), applicationNumber);

            clientDomainService.WhenToldTo(x => x.PerformQuery(Param.IsAny<FindClientByIdNumberQuery>())).Return<FindClientByIdNumberQuery>(y =>
            {
                y.Result =
                    new ServiceQueryResult<Services.Interfaces.ClientDomain.Models.ClientDetailsQueryResult>(
                        new List<Services.Interfaces.ClientDomain.Models.ClientDetailsQueryResult>() { null });
                return new SystemMessageCollection();
            });
            clientDomainService.WhenToldTo(x => x.PerformQuery(Param.IsAny<FindClientByPassportNumberQuery>()))
                .Return<FindClientByPassportNumberQuery>(y =>
                {
                    y.Result =
                        new ServiceQueryResult<Services.Interfaces.ClientDomain.Models.ClientDetailsQueryResult>(
                            new List<Services.Interfaces.ClientDomain.Models.ClientDetailsQueryResult> { null });
                    return new SystemMessageCollection();
                });
            var clientAddress = new GetClientFreeTextAddressQueryResult()
            {
                ClientKey = clientKey,
                AddressKey = 1,
                AddressTypeKey = 1
            };
            var freeTextqueryResult = new ServiceQueryResult<GetClientFreeTextAddressQueryResult>(
                new[]
                {
                    clientAddress
                });
            addressDomainService.WhenToldTo(ads => ads.PerformQuery(Param.IsAny<GetClientFreeTextAddressQuery>()))
                .Return<GetClientFreeTextAddressQuery>(rqst =>
                {
                    rqst.Result = freeTextqueryResult;
                    return new SystemMessageCollection();
                });
        };

        private Because of = () =>
        {
            domainProcess.Start(newPurchaseCreationModel, typeof(NewPurchaseApplicationAddedEvent).Name);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddNewPurchaseApplicationCommand)));
            domainProcess.HandleEvent(newPurchaseApplicationAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddNaturalPersonClientCommand))) { { "IdNumberOfAddedClient", applicant.IDNumber } };
            domainProcess.HandleEvent(naturalPersonClientAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddLeadApplicantToApplicationCommand)));
            domainProcess.HandleEvent(applicantAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, GetGuidForCommandType(typeof(AddEmployerCommand)))
            {
                { "EmployeeIdNumber", applicant.IDNumber }
            };
            domainProcess.HandleEvent(employerAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddUnconfirmedSalariedEmploymentToClientCommand)));
            domainProcess.HandleEvent(unconfirmedSalaryAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(DetermineApplicationHouseholdIncomeCommand)));
            domainProcess.HandleEvent(householdIncomeDeterminedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(SetApplicationEmploymentTypeCommand)));
            domainProcess.HandleEvent(applicationEmploymentSetEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(PriceNewBusinessApplicationCommand)));
            domainProcess.Handle(newBusinessPricedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(FundNewBusinessApplicationCommand)));
            domainProcess.Handle(newBusinessFundedEvent, metadata);
        };

        private It should_add_a_new_purchase_application = () =>
        {
            applicationDomainService.WasToldTo(ads => ads.PerformCommand(
                Param<AddNewPurchaseApplicationCommand>.Matches(m =>
                    m.NewPurchaseApplication.ApplicationType == newPurchaseCreationModel.ApplicationType &&
                        m.NewPurchaseApplication.Deposit == newPurchaseCreationModel.Deposit &&
                        m.NewPurchaseApplication.OriginationSource == newPurchaseCreationModel.OriginationSource &&
                        m.NewPurchaseApplication.Product == newPurchaseCreationModel.Product),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private Behaves_like<DomainProcessThatHasAddedANewApplicant> a_domain_process_that_has_added_a_new_applicant = () =>
        {
        };

        private Behaves_like<DomainProcessThatHasAddedApplicationDetails> a_new_purchase_process_that_has_added_the_application_details = () =>
        {
        };

        private It should_not_be_complete = () =>
        {
            applicationStateMachine.HasProcessCompletedWithCriticalPathFullyCaptured().ShouldBeFalse();
        };
    }
}
