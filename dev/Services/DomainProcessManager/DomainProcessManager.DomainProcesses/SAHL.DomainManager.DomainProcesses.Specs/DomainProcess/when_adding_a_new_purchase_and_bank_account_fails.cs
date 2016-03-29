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
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.Services.Interfaces.PropertyDomain.Commands;
using SAHL.Services.Interfaces.PropertyDomain.Events;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess
{
    public class when_adding_a_new_purchase_and_bank_account_fails : WithDomainServiceMocks
    {
        protected static NewPurchaseApplicationDomainProcess domainProcess;
        protected static NewPurchaseApplicationCreationModel newPurchaseCreationModel;

        protected static ApplicantModel applicant;
        protected static string vendorCode;
        protected static ComcorpApplicationPropertyDetailsModel comcorpApplicationPropertyDetail;
        private static ExternalVendorLinkedToApplicationEvent externalVendorLinkedEvent;
        private static FreeTextResidentialAddressLinkedToClientEvent freeTextResidentialAddressLinkedToClientEvent;
        private static FreeTextPostalAddressLinkedToClientEvent freeTextPostalAddressLinkedToClientEvent;
        private static ResidentialStreetAddressLinkedToClientEvent propertyAddressAddedAsResidentialAddressLinkedToClientEvent;
        private static ClientAddressAsPendingDomiciliumAddedEvent clientAddressAsPendingDomiciliumAddedEvent;
        private static MailingAddressAddedToApplicationEvent applicationMailingAddressEvent;
        private static DomiciliumAddressLinkedToApplicantEvent domiciliumAddressLinkedToApplicant;
        private static ComcorpOfferPropertyDetailsAddedEvent comcorpPropertyDetailAddedEvent;
        private static ApplicantAffordabilitiesAddedEvent applicantAffordabilitiesAddedEvent;
        private static DeclarationsAddedToApplicantEvent applicantDeclarationsAddedEvent;

        protected static int applicationNumber, clientKey, postalClientAddressKey, residentialClientAddressKey;
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
            clientDomiciliumKey = 33245;

            applicationDataManager.WhenToldTo(adm => adm.DoesSuppliedVendorExist(Param.IsAny<string>())).Return(true);
            clientDataManager.WhenToldTo(x => x.GetClientKeyForClientAddress(streetClientAddressKey)).Return(clientKey);

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

            var errorMessages = SystemMessageCollection.Empty();
            errorMessages.AddMessage(new SystemMessage("Error", SystemMessageSeverityEnum.Error));
            bankAccountDomainService.WhenToldTo(
                x => x.PerformCommand(Param.IsAny<LinkBankAccountToClientCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                .Return(errorMessages);
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

        private It should_not_add_the_application_debit_order = () =>
        {
            applicationDomainService.WasNotToldTo(x => x.PerformCommand(Param.IsAny<AddApplicationDebitOrderCommand>(),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_go_through_the_non_critical_error_state = () =>
        {
            applicationStateMachine.ContainsStateInBreadCrumb(ApplicationState.NonCriticalErrorOccured).ShouldBeTrue();
        };

        private It should_not_go_through_the_all_bank_accounts_added_state = () =>
        {
            applicationStateMachine.ContainsStateInBreadCrumb(ApplicationState.AllBankAccountsCaptured).ShouldBeFalse();
        };

        private Behaves_like<DomainProcessThatHasAddedAllAddresses> a_domain_process_that_has_added_all_addresses = () =>
        {
        };

        private Behaves_like<DomainProcessThatHasCompletedNonCriticalSteps> a_domain_process_that_has_completed_the_non_critical_steps = () =>
        {
        };

        private It should_be_in_completed_state = () =>
        {
            applicationStateMachine.IsInState(ApplicationState.Completed).ShouldBeTrue();
        };

        private It should_send_a_non_critical_error_email = () =>
        {
            communicationManager.WasToldTo(
                x => x.SendNonCriticalErrorsEmail(Param<ISystemMessageCollection>.Matches(m => m.HasErrors), applicationNumber));
        };
    }
}
