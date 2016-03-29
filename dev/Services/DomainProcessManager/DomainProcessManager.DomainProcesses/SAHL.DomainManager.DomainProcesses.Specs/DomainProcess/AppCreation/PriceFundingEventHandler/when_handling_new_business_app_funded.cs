using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.Services.Interfaces.FinancialDomain.Events;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.PriceFundingEventHandler
{
    public class when_handling_new_business_app_funded : WithNewPurchaseDomainProcess
    {
        private static NewBusinessApplicationFundedEvent newBusinessApplicationFundedEvent;
        private static int applicationNumber;
        private static NewPurchaseApplicationCreationModel applicationCreationModel;
        private static string vendorCode;

        private Establish context = () =>
        {
            vendorCode = "SAHL1";
            applicationNumber = 150;
            domainProcess.ProcessState = applicationStateMachine;

            newBusinessApplicationFundedEvent = new NewBusinessApplicationFundedEvent(new DateTime(2014, 01, 01).Date, applicationNumber);

            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(Core.BusinessModel.Enums.OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.DataModel = applicationCreationModel;

            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);

            var getClientFreeTextAddressQueryResult = new ServiceQueryResult<GetClientFreeTextAddressQueryResult>(
                        new GetClientFreeTextAddressQueryResult[] { });
            addressDomainService.WhenToldTo(ads => ads.PerformQuery(Param.IsAny<GetClientFreeTextAddressQuery>())).
                Return<GetClientFreeTextAddressQuery>(rqst => { rqst.Result = getClientFreeTextAddressQueryResult; return new SystemMessageCollection(); });

            var getClientStreetAddressQueryResult = new ServiceQueryResult<GetClientStreetAddressQueryResult>(
                        new GetClientStreetAddressQueryResult[] { });
            addressDomainService.WhenToldTo(ads => ads.PerformQuery(Param.IsAny<GetClientStreetAddressQuery>())).
                Return<GetClientStreetAddressQuery>(rqst => { rqst.Result = getClientStreetAddressQueryResult; return new SystemMessageCollection(); });
            
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(newBusinessApplicationFundedEvent, serviceRequestMetadata);
        };

        private It should_be_in_application_priced_state = () =>
        {
            applicationStateMachine.IsInState(ApplicationState.ApplicationPriced);
        };

        private It should_fire_the_application_funding_confimed_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationFundingConfirmed, Param.IsAny<Guid>()));
        };

        private It should_link_the_application_to_an_external_vendor = () =>
        {
            applicationDomainService.WasToldTo(a => a.PerformCommand(
                Param<LinkExternalVendorToApplicationCommand>.Matches(l => l.ApplicationNumber == applicationNumber && l.VendorCode == vendorCode),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_add_clients_free_text_as_postal_address = () =>
        {
            addressDomainService.WasToldTo(x => x.PerformCommand(Param.IsAny<LinkFreeTextAddressAsPostalAddressToClientCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_add_clients_bank_details = () =>
        {
            bankAccountDomainService.WasToldTo(x => x.PerformCommand(Param.IsAny<LinkBankAccountToClientCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_create_a_workflow_case = () =>
        {
            x2WorkflowManager.WasToldTo(x => x.CreateWorkflowCase(
                  Param.Is<int>(applicationNumber)
                , Param<DomainProcessServiceRequestMetadata>.Matches(m =>
                    m.ContainsKey(CoreGlobals.DomainProcessIdName) &&
                    m[CoreGlobals.DomainProcessIdName] == domainProcess.DomainProcessId.ToString()
                 )));
        };
    }
}