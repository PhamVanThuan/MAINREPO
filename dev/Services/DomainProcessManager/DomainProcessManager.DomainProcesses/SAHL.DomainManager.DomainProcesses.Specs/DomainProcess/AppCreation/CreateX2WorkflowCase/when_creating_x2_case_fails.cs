using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.Services.Interfaces.FinancialDomain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.CreateX2WorkflowCase
{
    public class when_creating_x2_case_fails : WithNewPurchaseDomainProcess
    {
        private static int applicationNumber;
        private static Exception runtimeException;
        private static Exception thrownException;
        private static NewBusinessApplicationFundedEvent newBusinessApplicationFundedEvent;
        private static NewPurchaseApplicationCreationModel applicationCreationModel;

        private Establish context = () =>
        {
            applicationNumber = 11432;
            runtimeException = new Exception("X2 case failed.");

            newBusinessApplicationFundedEvent = new NewBusinessApplicationFundedEvent(new DateTime(2014, 01, 01).Date, applicationNumber);
            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(Core.BusinessModel.Enums.OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            // Avoid coupling with methods handling applicants details
            applicationCreationModel.Applicants = new  List<ApplicantModel>();
            domainProcess.DataModel = applicationCreationModel;

            x2WorkflowManager.WhenToldTo(x => x.CreateWorkflowCase(applicationNumber, Param.IsAny<DomainProcessServiceRequestMetadata>()))
                             .Throw(runtimeException);

            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
        };

        private Because of = () =>
        {
            thrownException = Catch.Exception(() => domainProcess.HandleEvent(newBusinessApplicationFundedEvent, serviceRequestMetadata));
        };

        private It should_not_throw_an_exception = () =>
        {
            thrownException.ShouldBeNull();
        };

        private It should_Log_the_error_message = () =>
        {
            rawLogger.WasToldTo(x => x.LogError(Param.IsAny<LogLevel>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(),Param.IsAny<string>(), null));
        };

        private It should_add_the_exception_to_the_messages = () =>
        {
            applicationStateMachine.WasToldTo(x => x.AggregateMessages(Param<ISystemMessageCollection>.Matches(m =>
                m.ExceptionMessages().Any(y => y.Message.Equals(runtimeException.ToString())))));
        };

        private It should_send_support_email = () =>
        {
            communicationManager.WasToldTo(cm => cm.SendX2CaseCreationFailedSupportEmail(Param<ISystemMessageCollection>.Matches(m =>
                  m.ExceptionMessages().Any(y => y.Message.Equals(runtimeException.ToString())))
                , domainProcess.DomainProcessId
                , applicationNumber
             ));
        };
    }
}