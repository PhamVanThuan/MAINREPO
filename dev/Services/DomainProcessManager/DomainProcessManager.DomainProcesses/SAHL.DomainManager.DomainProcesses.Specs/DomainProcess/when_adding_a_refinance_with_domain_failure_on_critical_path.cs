using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess
{
    public class when_adding_a_refinance_with_domain_failure_on_critical_path : WithDomainServiceMocks
    {
        private static RefinanceApplicationDomainProcess domainProcess;
        private static RefinanceApplicationCreationModel refinanceCreationModel;
        private static Exception thrownException;
        private static ISystemMessageCollection errorMessages;

        private static ApplicantModel applicant;
        private static RefinanceApplicationAddedEvent RefinanceApplicationAddedEvent;
        private static NaturalPersonClientAddedEvent naturalPersonClientAddedEvent;

        private static int applicationNumber, clientKey, employmentKey;

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
            refinanceCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.RefinanceLoan) as RefinanceApplicationCreationModel;

            applicationNumber = 15006;
            clientKey = 554;
            employmentKey = 2673;

            var testDate = new DateTime(2014, 12, 1);
            RefinanceApplicationAddedEvent = DomainProcessTestHelper.GetRefinanceApplicationAddedEvent(applicationNumber, refinanceCreationModel, testDate);

            applicant = refinanceCreationModel.Applicants.First();
            naturalPersonClientAddedEvent = DomainProcessTestHelper.GetNaturalPersonClientAddedEvent(clientKey, applicant, testDate);

            errorMessages = SystemMessageCollection.Empty();
            errorMessages.AddMessage(new SystemMessage("Something went wrong", SystemMessageSeverityEnum.Error));
            clientDomainService.WhenToldTo(x => x.PerformCommand(Param.IsAny<AddNaturalPersonClientCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(errorMessages);

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

            var metadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, Guid.NewGuid());
            thrownException = Catch.Exception(() => domainProcess.HandleEvent(RefinanceApplicationAddedEvent, metadata));
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

        private It should_create_a_new_client = () =>
        {
            clientDomainService.WasToldTo(x => x.PerformCommand(
                Param<AddNaturalPersonClientCommand>.Matches(m =>
                m.NaturalPersonClient.IDNumber == applicant.IDNumber &&
                m.NaturalPersonClient.FirstName == applicant.FirstName &&
                m.NaturalPersonClient.Surname == applicant.Surname &&
                m.NaturalPersonClient.EmailAddress == applicant.EmailAddress),
                Param<DomainProcessServiceRequestMetadata>.Matches(m => m.ContainsKey("IdNumberOfAddedClient"))));
        };

        private It should_go_into_critical_error_state = () =>
        {
            applicationStateMachine.IsInState(ApplicationState.CriticalErrorOccured).ShouldBeTrue();
        };

        private It should_not_allow_moving_out_of_critical_error_state = () =>
        {
            invalidTransitionException = Catch.Exception(() => applicationStateMachine.TriggerEmploymentAdded(Guid.NewGuid(), employmentKey));
            invalidTransitionException.ShouldBeOfExactType(typeof(InvalidOperationException));
        };

        private It should_not_reverse_events_related_to_failed_application = () =>
        {
            applicationDataManager.WasToldTo(adm => adm.RollbackCriticalPathApplicationData(Param.Is<int>(applicationNumber), Param<IEnumerable<int>>.Matches(m => m.Count() == 0)));
        };

        private It should_throw_an_exception = () =>
        {
            thrownException.ShouldNotBeNull();
        };
    }
}