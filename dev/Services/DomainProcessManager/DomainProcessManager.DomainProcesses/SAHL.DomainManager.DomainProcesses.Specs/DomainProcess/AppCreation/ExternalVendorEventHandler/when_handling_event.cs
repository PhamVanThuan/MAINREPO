using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.ExternalVendorEventHandler
{
    public class when_handling_event : WithNewPurchaseDomainProcess
    {
        private static Dictionary<string, int> clientCollection;
        private static ExternalVendorLinkedToApplicationEvent externalVendorLinkedToApplicationEvent;
        private static int applicationNumber;
        private static int vendorKey, organisationStructureKey, clientKey;

        private Establish context = () =>
        {
            clientKey = 0;
            applicationNumber = 12;
            vendorKey = 35;
            organisationStructureKey = 11;

            VendorModel vendorModel = new VendorModel(vendorKey, "AC", organisationStructureKey, clientKey, 1);

            externalVendorLinkedToApplicationEvent = new ExternalVendorLinkedToApplicationEvent(applicationNumber, DateTime.Now, vendorModel.VendorKey, vendorModel.VendorCode,
                                                                                vendorModel.OrganisationStructureKey, vendorModel.LegalEntityKey, vendorModel.GeneralStatusKey);

            var address = ApplicationCreationTestHelper.PopulateFreeTextPostalAddressModel();
            var applicants = new List<ApplicantModel> {
                ApplicationCreationTestHelper.PopulateApplicantModel(new List<AddressModel> {address})
            };
            var propertyAddress = ApplicationCreationTestHelper.PopulatePropertyAddressModel();
            var mailingAddress = ApplicationCreationTestHelper.PopulateApplicationMailingAddressModel(address);
            var comcorpApplicationPropertyDetail = ApplicationCreationTestHelper.PopulateComcorpPropertyDetailsModel();

            var debitOrder = new Models.ApplicationDebitOrderModel(FinancialServicePaymentType.DebitOrderPayment, 25, ApplicationCreationTestHelper.PopulateBankAccountModel());

            domainProcess.DataModel = new NewPurchaseApplicationCreationModel(OfferStatus.Open, "Ref:123", 2, OriginationSource.Comcorp, "John", "Smith", applicants, 600000, 1500000, 2000000,
                                                                        comcorpApplicationPropertyDetail, 240, Product.NewVariableLoan, debitOrder, mailingAddress, "AC", 1000000,
                                                                        "6003082255441", "", "", propertyAddress);

            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            bankAccountDomainService.WhenToldTo(x => x.PerformCommand(Param.IsAny<LinkBankAccountToClientCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(SystemMessageCollection.Empty());
            combGuidGenerator.WhenToldTo(x => x.Generate()).Return(new Guid());

            clientCollection = new Dictionary<string, int>();
            clientCollection.Add(applicants.First().IDNumber, clientKey);
            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);


        };

        private Because of = () =>
        {
            domainProcess.Handle(externalVendorLinkedToApplicationEvent, serviceRequestMetadata);
        };

        private It should_fire_the_application_linking_to_external_vendor_confirmed_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(Arg.Is<ApplicationStateTransitionTrigger>(y => y == ApplicationStateTransitionTrigger.ApplicationLinkingToExternalVendorConfirmed),
                                                                            externalVendorLinkedToApplicationEvent.Id));
        };
    }
}