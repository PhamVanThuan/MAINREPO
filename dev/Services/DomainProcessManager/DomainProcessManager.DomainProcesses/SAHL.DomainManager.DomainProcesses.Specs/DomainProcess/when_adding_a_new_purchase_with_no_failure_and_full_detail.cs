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
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using SAHL.Services.Interfaces.BankAccountDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.Services.Interfaces.PropertyDomain.Commands;
using SAHL.Services.Interfaces.PropertyDomain.Events;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess
{
    public class when_adding_a_new_purchase_with_no_failure_and_full_detail : WithDomainServiceMocks
    {
        protected static NewPurchaseApplicationDomainProcess domainProcess;
        protected static NewPurchaseApplicationCreationModel newPurchaseCreationModel;

        protected static ApplicantModel applicant;
        protected static BankAccountModel bankAccount;
        protected static ApplicationDebitOrderModel applicationDebitOrder;
        protected static string vendorCode;
        protected static ComcorpApplicationPropertyDetailsModel comcorpApplicationPropertyDetail;
        private static ExternalVendorLinkedToApplicationEvent externalVendorLinkedEvent;
        private static FreeTextResidentialAddressLinkedToClientEvent freeTextResidentialAddressLinkedToClientEvent;
        private static FreeTextPostalAddressLinkedToClientEvent freeTextPostalAddressLinkedToClientEvent;
        private static ResidentialStreetAddressLinkedToClientEvent propertyAddressAddedAsResidentialAddressLinkedToClientEvent;
        private static BankAccountLinkedToClientEvent bankAccountLinkedToClientEvent;
        private static ClientAddressAsPendingDomiciliumAddedEvent clientAddressAsPendingDomiciliumAddedEvent;
        private static MailingAddressAddedToApplicationEvent applicationMailingAddressEvent;
        private static DomiciliumAddressLinkedToApplicantEvent domiciliumAddressLinkedToApplicant;
        private static DebitOrderAddedToApplicationEvent debitOrderAddedToApplicationEvent;
        private static ComcorpOfferPropertyDetailsAddedEvent comcorpPropertyDetailAddedEvent;
        private static ApplicantAffordabilitiesAddedEvent applicantAffordabilitiesAddedEvent;
        private static DeclarationsAddedToApplicantEvent applicantDeclarationsAddedEvent;

        protected static int applicationNumber, clientKey, postalClientAddressKey, residentialClientAddressKey, bankAccountKey, clientBankAccountKey;
        protected static int streetClientAddressKey;
        protected static int clientDomiciliumKey;

        protected static ServiceRequestMetadata metadata;

        protected static IRawLogger rawLogger;
        protected static ILoggerSource loggerSource;
        protected static ILoggerAppSource loggerAppSource;

        protected Establish context = () =>
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

            applicant = newPurchaseCreationModel.Applicants.First();

            applicationNumber = 15006;
            clientKey = 554;
            postalClientAddressKey = 4000;
            residentialClientAddressKey = 3324;
            streetClientAddressKey = 9923;
            bankAccountKey = 55345;
            clientBankAccountKey = 13;
            clientDomiciliumKey = 33245;

            var testDate = new DateTime(2014, 10, 9);

            vendorCode = newPurchaseCreationModel.VendorCode;
            var vendor = new Services.Interfaces.ApplicationDomain.Models.VendorModel(22, vendorCode, 2, 678, 1);
            externalVendorLinkedEvent = DomainProcessTestHelper.GetExternalVendorLinkedEvent(applicationNumber, vendor, testDate);

            var freeTextAddressModel = applicant.Addresses.First() as Models.FreeTextAddressModel;
            freeTextResidentialAddressLinkedToClientEvent = DomainProcessTestHelper.GetFreeTextResidentialAddressLinkedToClientEvent(
                clientKey,
                residentialClientAddressKey,
                freeTextAddressModel,
                testDate);

            propertyAddressAddedAsResidentialAddressLinkedToClientEvent = DomainProcessTestHelper
                .GetResidentialStreeAddressLinkedToClientEventFromPropertyAddress(
                    clientKey,
                    streetClientAddressKey,
                    newPurchaseCreationModel.PropertyAddress,
                    testDate);

            freeTextPostalAddressLinkedToClientEvent = DomainProcessTestHelper.GetFreeTextPostalAddressLinkedToClientEvent(
                clientKey,
                postalClientAddressKey,
                freeTextAddressModel,
                testDate);

            applicationMailingAddressEvent = DomainProcessTestHelper.GetApplicationMailingAddressAddedEvent(applicationNumber,
                streetClientAddressKey,
                newPurchaseCreationModel.ApplicationMailingAddress,
                testDate);

            clientAddressAsPendingDomiciliumAddedEvent = DomainProcessTestHelper.GetClientDomiciliumAddedEvent(streetClientAddressKey,
                clientDomiciliumKey,
                testDate);

            clientDomainService.WhenToldTo(x => x.PerformQuery(Param.IsAny<FindClientByIdNumberQuery>())).Return<FindClientByIdNumberQuery>(y =>
            {
                y.Result = new ServiceQueryResult<ClientDetailsQueryResult>(new List<ClientDetailsQueryResult>() { null });
                return new SystemMessageCollection();
            });

            clientDomainService.WhenToldTo(x => x.PerformQuery(Param.IsAny<FindClientByPassportNumberQuery>()))
                .Return<FindClientByPassportNumberQuery>(y =>
                {
                    y.Result = new ServiceQueryResult<ClientDetailsQueryResult>(new List<ClientDetailsQueryResult> { null });
                    return new SystemMessageCollection();
                });

            bankAccount = applicant.BankAccounts.First();
            bankAccountLinkedToClientEvent = DomainProcessTestHelper.GetBankAccountLinkedToClientEvent(bankAccountKey,
                clientKey,
                clientBankAccountKey,
                bankAccount,
                testDate);

            applicationDebitOrder = newPurchaseCreationModel.ApplicationDebitOrder;
            debitOrderAddedToApplicationEvent = DomainProcessTestHelper.GetDebitOrderAddedToApplicationEvent(applicationNumber,
                clientBankAccountKey,
                applicationDebitOrder,
                testDate);

            domiciliumAddressLinkedToApplicant = DomainProcessTestHelper.GetDomiciliumLinkedToApplicantEvent(2214,
                clientKey,
                applicationNumber,
                clientDomiciliumKey,
                testDate);

            comcorpApplicationPropertyDetail = newPurchaseCreationModel.ComcorpApplicationPropertyDetail;
            comcorpPropertyDetailAddedEvent = DomainProcessTestHelper.GetComcorpPropertyDetailAddedEvent(comcorpApplicationPropertyDetail, testDate);

            applicantAffordabilitiesAddedEvent = DomainProcessTestHelper.GetApplicantAffordabilitiesAddedEvent(clientKey,
                applicationNumber,
                applicant.Affordabilities.ToList(),
                testDate);

            applicantDeclarationsAddedEvent = DomainProcessTestHelper.GetApplicantDeclarationsAddedEvent(clientKey,
                applicationNumber,
                applicant.ApplicantDeclarations,
                testDate);

            addressDomainService.WhenToldTo<IAddressDomainServiceClient>(ads => ads.PerformQuery(Param.IsAny<GetClientStreetAddressQuery>()))
                .Callback<GetClientStreetAddressQuery>(rqst =>
                {
                    rqst.Result = new ServiceQueryResult<GetClientStreetAddressQueryResult>(new GetClientStreetAddressQueryResult[] { });
                });

            addressDomainService.WhenToldTo<IAddressDomainServiceClient>(ads => ads.PerformQuery(Param.IsAny<GetClientFreeTextAddressQuery>()))
                .Callback<GetClientFreeTextAddressQuery>(rqst =>
                {
                    rqst.Result = new ServiceQueryResult<GetClientFreeTextAddressQueryResult>(new GetClientFreeTextAddressQueryResult[] { });
                });
            clientDataManager.WhenToldTo(x => x.GetClientKeyForClientAddress(streetClientAddressKey)).Return(clientKey);
        };

        private Because of = () =>
        {
            domainProcess.Start(newPurchaseCreationModel, typeof(NewPurchaseApplicationAddedEvent).Name);
            DomainProcessTestHelper.GetNewPurchaseDomainProcessPastCriticalPath(domainProcess,
                newPurchaseCreationModel,
                applicationNumber,
                GetGuidForCommandType);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(LinkFreeTextAddressAsResidentialAddressToClientCommand)));
            domainProcess.HandleEvent(freeTextResidentialAddressLinkedToClientEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(LinkFreeTextAddressAsPostalAddressToClientCommand)));
            domainProcess.HandleEvent(freeTextPostalAddressLinkedToClientEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(LinkStreetAddressAsResidentialAddressToClientCommand)));
            domainProcess.HandleEvent(propertyAddressAddedAsResidentialAddressLinkedToClientEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddClientAddressAsPendingDomiciliumCommand)));
            domainProcess.HandleEvent(clientAddressAsPendingDomiciliumAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(LinkDomiciliumAddressToApplicantCommand)));
            domainProcess.HandleEvent(domiciliumAddressLinkedToApplicant, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddApplicationMailingAddressCommand)));
            domainProcess.HandleEvent(applicationMailingAddressEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(LinkBankAccountToClientCommand)));
            domainProcess.HandleEvent(bankAccountLinkedToClientEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddApplicationDebitOrderCommand)));
            domainProcess.HandleEvent(debitOrderAddedToApplicationEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(LinkExternalVendorToApplicationCommand)));
            domainProcess.HandleEvent(externalVendorLinkedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddComcorpOfferPropertyDetailsCommand)));
            domainProcess.HandleEvent(comcorpPropertyDetailAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddApplicantAffordabilitiesCommand)));
            domainProcess.HandleEvent(applicantAffordabilitiesAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddApplicantDeclarationsCommand)));
            domainProcess.HandleEvent(applicantDeclarationsAddedEvent, metadata);
        };

        private Behaves_like<DomainProcessThatHasAddedAllAddresses> a_domain_process_that_has_added_all_addresses = () =>
        {
        };

        private Behaves_like<DomainProcessThatHasAddedBankAccounts> a_domain_process_that_has_added_all_bank_accounts = () =>
        {
        };

        private Behaves_like<DomainProcessThatHasCompletedNonCriticalSteps> a_domain_process_that_has_completed_the_non_critical_steps = () =>
        {
        };

        private It should_be_in_completed_state = () =>
        {
            applicationStateMachine.IsInState(ApplicationState.Completed).ShouldBeTrue();
        };

        private It should_not_send_a_non_critical_error_email = () =>
        {
            communicationManager.WasNotToldTo(
                x => x.SendNonCriticalErrorsEmail(Param<ISystemMessageCollection>.Matches(m => m.HasErrors), applicationNumber));
        };
    }
}
