using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.Services.Interfaces.PropertyDomain.Commands;
using System;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess
{
    public class when_adding_a_new_purchase_and_non_critical_path_all_fail : WithDomainServiceMocks
    {
        private static NewPurchaseApplicationDomainProcess domainProcess;
        private static NewPurchaseApplicationCreationModel newPurchaseCreationModel;
        private static int applicationNumber;
        private static IRawLogger rawLogger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;

        protected Establish context = () =>
        {
            rawLogger = An<IRawLogger>();
            loggerSource = An<ILoggerSource>();
            loggerAppSource = An<ILoggerAppSource>();
            applicationStateMachine = new ApplicationStateMachine();

            domainProcess = new NewPurchaseApplicationDomainProcess(
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
            newPurchaseCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            applicationNumber = 15006;

            var errors = SystemMessageCollection.Empty();
            errors.AddMessage(new SystemMessage("Error!!", SystemMessageSeverityEnum.Error));

            applicationDomainService.WhenToldTo(x => x.PerformCommand(
                Param.IsAny<LinkExternalVendorToApplicationCommand>(),
                Param.IsAny<IServiceRequestMetadata>())).Return(errors);
            applicationDomainService.WhenToldTo(x => x.PerformCommand(
                Param.IsAny<AddApplicantAffordabilitiesCommand>(),
                Param.IsAny<IServiceRequestMetadata>())).Return(errors);
            applicationDomainService.WhenToldTo(x => x.PerformCommand(
                Param.IsAny<AddApplicantDeclarationsCommand>(),
                Param.IsAny<IServiceRequestMetadata>())).Return(errors);

            addressDomainService.WhenToldTo(x => x.PerformCommand(
                Param.IsAny<LinkFreeTextAddressAsResidentialAddressToClientCommand>(),
                Param.IsAny<IServiceRequestMetadata>())).Return(errors);
            addressDomainService.WhenToldTo(x => x.PerformCommand(
                Param.IsAny<LinkFreeTextAddressAsPostalAddressToClientCommand>(),
                Param.IsAny<IServiceRequestMetadata>())).Return(errors);
            addressDomainService.WhenToldTo(x => x.PerformCommand(
                Param.IsAny<LinkStreetAddressAsResidentialAddressToClientCommand>(),
                Param.IsAny<IServiceRequestMetadata>())).Return(errors);

            bankAccountDomainService.WhenToldTo(x => x.PerformCommand(
                Param.IsAny<LinkBankAccountToClientCommand>(),
                Param.IsAny<IServiceRequestMetadata>())).Return(errors);

            propertyDomainService.WhenToldTo(x => x.PerformCommand(
                Param.IsAny<AddComcorpOfferPropertyDetailsCommand>(),
                Param.IsAny<IServiceRequestMetadata>())).Return(errors);

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
            domainProcess.Start(newPurchaseCreationModel, typeof(NewPurchaseApplicationAddedEvent).Name);
            DomainProcessTestHelper.GetNewPurchaseDomainProcessPastCriticalPath(domainProcess, newPurchaseCreationModel, applicationNumber, GetGuidForCommandType);
        };

        private It should_create_the_x2_case = () =>
        {
            x2WorkflowManager.WasToldTo(x => x.CreateWorkflowCase(
                  applicationNumber
                , Param<DomainProcessServiceRequestMetadata>.Matches(
                    m => m.ContainsKey(CoreGlobals.DomainProcessIdName) &&
                    m[CoreGlobals.DomainProcessIdName] == domainProcess.DomainProcessId.ToString())
            ));
        };

        private It should_go_into_the_non_critical_state = () =>
        {
            applicationStateMachine.ContainsStateInBreadCrumb(ApplicationState.NonCriticalErrorOccured).ShouldBeTrue();
        };

        private It should_be_in_completed_state = () =>
        {
            applicationStateMachine.IsInState(ApplicationState.Completed).ShouldBeTrue();
        };

        private It should_send_a_non_critical_error_email = () =>
        {
            communicationManager.WasToldTo(x => x.SendNonCriticalErrorsEmail(Param<ISystemMessageCollection>.Matches(m => m.HasErrors), applicationNumber));
        };
    }
}