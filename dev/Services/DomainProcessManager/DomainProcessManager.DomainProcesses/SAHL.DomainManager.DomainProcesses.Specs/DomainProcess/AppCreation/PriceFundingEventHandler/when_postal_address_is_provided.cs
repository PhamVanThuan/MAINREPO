using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.Services.Interfaces.FinancialDomain.Events;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.Core;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.PriceFundingEventHandler
{
    public class when_postal_address_is_provided : WithNewPurchaseDomainProcess
    {
        private static NewBusinessApplicationFundedEvent newBusinessApplicationFundedEvent;
        private static int applicationNumber, clientKey;
        private static Dictionary<string, int> clientCollection;

        private Establish context = () =>
        {
            applicationNumber = 12;

            newBusinessApplicationFundedEvent = new NewBusinessApplicationFundedEvent(new DateTime(2014, 01, 01).Date, applicationNumber);

            clientKey = 100;

            var newPurchaseCreationModel =
                ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            var applicant = newPurchaseCreationModel.Applicants.First();
            var addresses = applicant.Addresses.ToList();
            addresses.Add(ApplicationCreationTestHelper.PopulateFreeTextPostalAddressModel());
            applicant.Addresses = addresses;

            domainProcess.DataModel = newPurchaseCreationModel;
            applicationStateMachine.WhenToldTo(asm => asm.ApplicationNumber).Return(applicationNumber);

            bankAccountDomainService.WhenToldTo(
                x => x.PerformCommand(Param.IsAny<LinkBankAccountToClientCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                .Return(SystemMessageCollection.Empty());
            combGuidGenerator.WhenToldTo(x => x.Generate()).Return(new Guid());

            clientCollection = new Dictionary<string, int>();
            clientCollection.Add(applicant.IDNumber, clientKey);
            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);

            var freeTextqueryResult = new ServiceQueryResult<GetClientFreeTextAddressQueryResult>(
                new GetClientFreeTextAddressQueryResult[] { });
            addressDomainService.WhenToldTo(ads => ads.PerformQuery(Param.IsAny<GetClientFreeTextAddressQuery>()))
                .Return<GetClientFreeTextAddressQuery>(rqst =>
                {
                    rqst.Result = freeTextqueryResult;
                    return new SystemMessageCollection();
                });

            var streetqueryResult = new ServiceQueryResult<GetClientStreetAddressQueryResult>(
                new GetClientStreetAddressQueryResult[] { });
            addressDomainService.WhenToldTo(ads => ads.PerformQuery(Param.IsAny<GetClientStreetAddressQuery>()))
                .Return<GetClientStreetAddressQuery>(rqst =>
                {
                    rqst.Result = streetqueryResult;
                    return new SystemMessageCollection();
                });
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(newBusinessApplicationFundedEvent, serviceRequestMetadata);
        };

        private It should_fire_the_application_funding_confimed_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationFundingConfirmed, Param.IsAny<Guid>()));
        };

        private It should_create_a_workflow_case = () =>
        {
            x2WorkflowManager.WasToldTo(x => x.CreateWorkflowCase(
                 Param.Is<int>(applicationNumber)
               , Param<DomainProcessServiceRequestMetadata>.Matches(m =>
                    m.ContainsKey(CoreGlobals.DomainProcessIdName) &&
                    m[CoreGlobals.DomainProcessIdName] == domainProcess.DomainProcessId.ToString()
                 )
            ));
        };

        private It should_add_clients_street_address = () =>
        {
            addressDomainService.WasToldTo(x => x.PerformCommand(Param.IsAny<LinkStreetAddressAsResidentialAddressToClientCommand>(),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_add_clients_postal_address = () =>
        {
            addressDomainService.WasToldTo(x => x.PerformCommand(Param.IsAny<LinkFreeTextAddressAsPostalAddressToClientCommand>(),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_add__street_address_as_clients_postal_address = () =>
        {
            addressDomainService.WasNotToldTo(x => x.PerformCommand(Param.IsAny<LinkStreetAddressAsPostalAddressToClientCommand>(),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_add_clients_bank_details = () =>
        {
            bankAccountDomainService.WasToldTo(x => x.PerformCommand(Param.IsAny<LinkBankAccountToClientCommand>(),
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
