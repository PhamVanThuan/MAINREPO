using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess
{
    public class when_adding_a_new_purchase_throws_error_on_critical_path : WithDomainServiceMocks
    {
        protected static NewPurchaseApplicationDomainProcess domainProcess;
        protected static NewPurchaseApplicationCreationModel newPurchaseCreationModel;
        private static Exception thrownException;

        private static ApplicantModel applicant;
        private static NewPurchaseApplicationAddedEvent newPurchaseApplicationAddedEvent;
        private static int applicationNumber;
        private static DomainProcessServiceRequestMetadata metadata;

        private static Exception invalidTransitionException;
        private static IRawLogger rawLogger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;

        private Establish context = () =>
        {
            rawLogger = An<IRawLogger>();
            loggerSource = An<ILoggerSource>();
            loggerAppSource = An<ILoggerAppSource>();
            applicationStateMachine = new ApplicationStateMachine();
            domainRuleManager = An<IDomainRuleManager<ApplicationCreationModel>>();

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
            applicationNumber = 15006;
            newPurchaseCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            newPurchaseApplicationAddedEvent = DomainProcessTestHelper.GetNewPurchaseApplicationAddedEvent(applicationNumber, newPurchaseCreationModel, new DateTime(2014, 11, 9));

            applicant = newPurchaseCreationModel.Applicants.First();

            clientDomainService.WhenToldTo(x => x.PerformQuery(Param.IsAny<FindClientByIdNumberQuery>())).Return<FindClientByIdNumberQuery>(y =>
            {
                y.Result = null;
                return new SystemMessageCollection();
            });
        };

        private Because of = () =>
        {
            domainProcess.Start(newPurchaseCreationModel, typeof(NewPurchaseApplicationAddedEvent).Name);

            metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, Guid.NewGuid());
            thrownException = Catch.Exception(() => domainProcess.HandleEvent(newPurchaseApplicationAddedEvent, metadata));
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

        private It should_set_the_application_number_on_the_state_machine = () =>
        {
            applicationStateMachine.ApplicationNumber.ShouldEqual(applicationNumber);
        };

        private It should_go_into_critical_error_state = () =>
        {
            applicationStateMachine.IsInState(ApplicationState.CriticalErrorOccured).ShouldBeTrue();
        };

        private It should_not_allow_moving_out_of_critical_error_state = () =>
        {
            invalidTransitionException = Catch.Exception(() => applicationStateMachine.TriggerEmploymentAdded(Guid.NewGuid(), 123));
            invalidTransitionException.ShouldBeOfExactType(typeof(InvalidOperationException));
        };

        private It should_invoke_action_to_reverse_events_of_failed_application = () =>
        {
            applicationDataManager.WasToldTo(adm => adm.RollbackCriticalPathApplicationData(Param.Is<int>(applicationNumber), Param.IsAny<IEnumerable<int>>()));
        };

        private It should_throw_an_exception = () =>
        {
            thrownException.ShouldNotBeNull();
        };
    }
}