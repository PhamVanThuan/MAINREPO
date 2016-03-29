using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.ApplicationDebitOrderHandler
{
    public class when_handling_event : WithNewPurchaseDomainProcess
    {
        private static NewPurchaseApplicationCreationModel applicationCreationModel;
        private static DebitOrderAddedToApplicationEvent debitOrderAddedToApplicationEvent;
        private static int applicationNumber;
        private static int clientBankAccountKey;

        private Establish context = () =>
        {
            clientBankAccountKey = 5656;
            applicationNumber = 123;

            var applicationDebitOrder = new ApplicationDebitOrderModel(FinancialServicePaymentType.DebitOrderPayment, 27, ApplicationCreationTestHelper.PopulateBankAccountModel());

            debitOrderAddedToApplicationEvent = new DebitOrderAddedToApplicationEvent(DateTime.Now, applicationDebitOrder.PaymentType, applicationDebitOrder.DebitOrderDay,
                                                                                            clientBankAccountKey, applicationNumber);

            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);

            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.DataModel = applicationCreationModel;

            combGuidGenerator.WhenToldTo(x => x.Generate()).Return(new Guid());
        };

        private Because of = () =>
        {
            domainProcess.Handle(debitOrderAddedToApplicationEvent, serviceRequestMetadata);
        };

        private It should_fire_the_correct_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(Arg.Is<ApplicationStateTransitionTrigger>(y => y == ApplicationStateTransitionTrigger.ApplicationDebitOrderCaptureConfirmed), Param.IsAny<Guid>()));
        };
    }
}