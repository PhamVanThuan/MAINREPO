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
    public class when_adding_a_new_purchase_containing_assets : WithDomainServiceMocks
    {
        protected static NewPurchaseApplicationDomainProcess domainProcess;
        protected static NewPurchaseApplicationCreationModel newPurchaseCreationModel;

        protected static ApplicantModel applicant;
        protected static BankAccountModel bankAccount;
        protected static ApplicationDebitOrderModel applicationDebitOrder;
        protected static List<ApplicantAssetLiabilityModel> assetLiabilities;
        private static Models.FreeTextAddressModel fixedPropertyAddressModel;
        protected static string vendorCode;
        protected static ComcorpApplicationPropertyDetailsModel comcorpApplicationPropertyDetail;
        private static FixedLongTermInvestmentLiabilityAddedToClientEvent fixedLongTermInvestmentAddedEvent;
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

        private static LiabilityLoanAddedToClientEvent liabilityLoanEvent;
        private static LiabilitySuretyAddedToClientEvent liabilitySuretyAddedEvent;
        private static OtherAssetAddedToClientEvent otherAssetAddedEvent;
        private static LifeAssuranceAssetAddedToClientEvent lifeAssuranceAddedEvent;
        private static FixedPropertyAssetAddedToClientEvent fixedPropertyAddedEvent;
        private static InvestmentAssetAddedToClientEvent listedInvestmentAdded;
        private static InvestmentAssetAddedToClientEvent unlistedInvestmentAdded;

        protected static int applicationNumber, clientKey, postalClientAddressKey, residentialClientAddressKey, bankAccountKey, clientBankAccountKey;
        protected static int streetClientAddressKey;
        protected static int clientDomiciliumKey;
        private static int fixedPropertyClientAddressKey;
        protected static int fixedPropertyAddressKey;

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
            assetLiabilities = ApplicationCreationTestHelper.PopulateApplicantAssetLiabilities();
            applicant.ApplicantAssetLiabilities = assetLiabilities;
            var fixedPropertyAsset =
                assetLiabilities.First(x => x.AssetLiabilityType == AssetLiabilityType.FixedProperty) as ApplicantFixedPropertyAssetModel;
            fixedPropertyAddressModel = fixedPropertyAsset.Address as Models.FreeTextAddressModel;
            var addresses = applicant.Addresses.ToList();
            addresses.Add(fixedPropertyAddressModel);
            applicant.Addresses = addresses;

            applicationNumber = 15006;
            clientKey = 554;
            postalClientAddressKey = 4000;
            residentialClientAddressKey = 3324;
            streetClientAddressKey = 9923;
            bankAccountKey = 55345;
            clientBankAccountKey = 13;
            clientDomiciliumKey = 33245;
            fixedPropertyClientAddressKey = 8837;
            fixedPropertyAddressKey = 999;

            var testDate = new DateTime(2014, 10, 9);

            vendorCode = newPurchaseCreationModel.VendorCode;
            var vendor = new Services.Interfaces.ApplicationDomain.Models.VendorModel(22, vendorCode, 2, 678, 1);
            externalVendorLinkedEvent = DomainProcessTestHelper.GetExternalVendorLinkedEvent(applicationNumber, vendor, testDate);

            var freeTextAddressModel =
                applicant.Addresses.First(add => add.AddressType == AddressType.Postal && add.FreeText1 != fixedPropertyAsset.Address.FreeText1) as
                    Models.FreeTextAddressModel;
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

            var freeTextPropertyAssetAddressModel = applicant.Addresses.First(x => x.FreeText1 == fixedPropertyAsset.Address.FreeText1);
            freeTextResidentialAddressLinkedToClientEvent = DomainProcessTestHelper.GetFreeTextResidentialAddressLinkedToClientEvent(
                clientKey,
                fixedPropertyClientAddressKey,
                freeTextPropertyAssetAddressModel as Models.FreeTextAddressModel,
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

            addressDomainService.WhenToldTo<IAddressDomainServiceClient>(ads => ads.PerformQuery(
                Param<GetClientFreeTextAddressQuery>.Matches(m => m.Address.FreeText1 == fixedPropertyAsset.Address.FreeText1)))
                .Callback<GetClientFreeTextAddressQuery>(rqst =>
                {
                    rqst.Result = new ServiceQueryResult<GetClientFreeTextAddressQueryResult>(new[]
                    {
                        new GetClientFreeTextAddressQueryResult
                        {
                            AddressKey = fixedPropertyAddressKey,
                            ClientAddressKey = fixedPropertyClientAddressKey,
                            AddressTypeKey = (int)fixedPropertyAsset.Address.AddressType,
                            ClientKey = clientKey
                        }
                    });
                });
            addressDomainService.WhenToldTo<IAddressDomainServiceClient>(ads => ads.PerformQuery(
                Param<GetClientFreeTextAddressQuery>.Matches(m => m.Address.FreeText1 != fixedPropertyAsset.Address.FreeText1)))
                .Callback<GetClientFreeTextAddressQuery>(rqst =>
                {
                    rqst.Result = new ServiceQueryResult<GetClientFreeTextAddressQueryResult>(new GetClientFreeTextAddressQueryResult[] { });
                });
            clientDataManager.WhenToldTo(x => x.GetClientKeyForClientAddress(streetClientAddressKey)).Return(clientKey);

            fixedLongTermInvestmentAddedEvent = DomainProcessTestHelper.GetFixedLongTermInvestmentAddedEvent(
                assetLiabilities.First(x => x.AssetLiabilityType == AssetLiabilityType.FixedLongTermInvestment),
                testDate);
            liabilityLoanEvent = DomainProcessTestHelper.GetLiabilityLoanAddedEvent(
                assetLiabilities.First(x => x.AssetLiabilityType == AssetLiabilityType.LiabilityLoan),
                testDate);
            liabilitySuretyAddedEvent = DomainProcessTestHelper.GetLiabilitySuretyAddedEvent(
                assetLiabilities.First(x => x.AssetLiabilityType == AssetLiabilityType.LiabilitySurety),
                1234,
                6657,
                testDate);
            otherAssetAddedEvent = DomainProcessTestHelper.GetOtherAssetAddedToClientEvent(
                assetLiabilities.First(x => x.AssetLiabilityType == AssetLiabilityType.OtherAsset),
                123,
                testDate);
            lifeAssuranceAddedEvent = DomainProcessTestHelper.GetLifeAssuranceAddedEvent(
                assetLiabilities.First(x => x.AssetLiabilityType == AssetLiabilityType.LifeAssurance),
                testDate);
            fixedPropertyAddedEvent = DomainProcessTestHelper.GetFixedPropertyAssetAddedEvent(
                assetLiabilities.First(x => x.AssetLiabilityType == AssetLiabilityType.FixedProperty),
                fixedPropertyAddressKey,
                testDate);
            listedInvestmentAdded = DomainProcessTestHelper.GetInvestmentAssetAddedEvent(
                assetLiabilities.First(x => x.AssetLiabilityType == AssetLiabilityType.ListedInvestments),
                AssetInvestmentType.ListedInvestments,
                testDate);
            unlistedInvestmentAdded = DomainProcessTestHelper.GetInvestmentAssetAddedEvent(
                assetLiabilities.First(x => x.AssetLiabilityType == AssetLiabilityType.UnlistedInvestments),
                AssetInvestmentType.UnlistedInvestments,
                testDate);
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

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddFixedLongTermInvestmentLiabilityToClientCommand)));
            domainProcess.HandleEvent(fixedLongTermInvestmentAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddLiabilityLoanToClientCommand)));
            domainProcess.HandleEvent(liabilityLoanEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddLiabilitySuretyToClientCommand)));
            domainProcess.HandleEvent(liabilitySuretyAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddOtherAssetToClientCommand)));
            domainProcess.HandleEvent(otherAssetAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddLifeAssuranceAssetToClientCommand)));
            domainProcess.HandleEvent(lifeAssuranceAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddFixedPropertyAssetToClientCommand)));
            domainProcess.HandleEvent(fixedPropertyAddedEvent, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddInvestmentAssetToClientCommand)));
            domainProcess.HandleEvent(listedInvestmentAdded, metadata);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId,
                GetGuidForCommandType(typeof(AddInvestmentAssetToClientCommand)));
            domainProcess.HandleEvent(unlistedInvestmentAdded, metadata);
        };

      

        private It should_be_in_completed_state = () =>
        {
            applicationStateMachine.IsInState(ApplicationState.Completed).ShouldBeTrue();
        };
    }
}
