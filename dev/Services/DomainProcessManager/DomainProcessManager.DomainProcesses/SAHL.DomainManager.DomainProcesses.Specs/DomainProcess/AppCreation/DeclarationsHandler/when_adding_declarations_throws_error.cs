﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.DeclarationsHandler
{
    public class when_adding_declarations_throws_error : WithNewPurchaseDomainProcess
    {
        private static NewPurchaseApplicationCreationModel newPurchaseCreationModel;
        private static List<ApplicantDeclarationsModel> applicantDeclarations;
        private static int clientKey, applicationNumber;
        private static string identityNumber;
        private static Exception runtimeException;

        private static Exception thrownException;
        private static Guid correlationId;
        private static string friendlyErrorMessage;

        private Establish context = () =>
        {
            clientKey = 15;
            applicationNumber = 105;

            correlationId = Guid.Parse("{528CF028-7990-4228-B1F9-4644341689DB}");
            applicantDeclarations = new List<ApplicantDeclarationsModel> {
                new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.DeclaredInsolventQuestionKey, OfferDeclarationAnswer.No, null),
                new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.UnderAdministrationOrderQuestionKey, OfferDeclarationAnswer.No, null),
                new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.CurrentlyUnderDebtCounsellingQuestionKey, OfferDeclarationAnswer.No, null),
                new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.PermissionToConductCreditCheckQuestionKey, OfferDeclarationAnswer.Yes, null),
            };
            newPurchaseCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            newPurchaseCreationModel.Applicants.First().ApplicantDeclarations = applicantDeclarations;
            domainProcess.ProcessState = applicationStateMachine;
            domainProcess.DataModel = newPurchaseCreationModel;

            identityNumber = domainProcess.DataModel.Applicants.First().IDNumber;
            var clientCollection = new Dictionary<string, int> { { identityNumber, clientKey } };
            friendlyErrorMessage = string.Format("Declarations could not be added for applicant with ID number: {0}", identityNumber);

            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);

            combGuidGenerator.WhenToldTo(x => x.Generate()).Return(correlationId);

            runtimeException = new Exception("Declarations exception");
            applicationDomainService.WhenToldTo(x => x.PerformCommand(Param.IsAny<AddApplicantDeclarationsCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                                                        .Throw(runtimeException);
        };

        private Because of = () =>
        {
            thrownException = Catch.Exception(() => domainProcess.AddDeclarations(applicationStateMachine, domainProcess.DataModel.Applicants));
        };

        private It should_not_throw_an_exception = () =>
        {
            thrownException.ShouldBeNull();
        };

        private It should_fire_the_non_critical_error_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.NonCriticalErrorReported, Param.IsAny<Guid>()));
        };

        private It should_record_the_command_failure = () =>
        {
            applicationStateMachine.WasToldTo(x => x.RecordCommandFailed(correlationId));
        };

        private It should_add_the_error_to_the_state_machine_messages = () =>
        {
            applicationStateMachine.WasToldTo(x => x.AggregateMessages(Param<ISystemMessageCollection>.Matches(m =>
                m.ErrorMessages().Any(y => y.Message.Contains(identityNumber)) &&
                m.ExceptionMessages().Any(y => y.Message.Contains(runtimeException.ToString())))));
        };

        private It should_log_the_error_message = () =>
        {
            rawLogger.WasToldTo(x => x.LogError(Param.IsAny<LogLevel>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()
             , Param<string>.Matches(m => m.Contains(friendlyErrorMessage)), null));
        };
    }
}