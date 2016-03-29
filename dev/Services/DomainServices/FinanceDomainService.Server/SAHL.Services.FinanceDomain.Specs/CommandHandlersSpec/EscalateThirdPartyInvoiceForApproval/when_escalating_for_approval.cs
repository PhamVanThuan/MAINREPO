using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.EscalateThirdPartyInvoiceForApproval
{
    public class when_escalating_for_approval : WithCoreFakes
    {
        private static EscalateThirdPartyInvoiceForApprovalCommand command;
        public static int thirdPartyInvoiceKey { get; private set; }
        private static EscalateThirdPartyInvoiceForApprovalCommandHandler handler;
        private static IThirdPartyInvoiceDataManager dataManager;
        private static int uosKey;

        private Establish context = () =>
        {
            uosKey = 123456;
            dataManager = An<IThirdPartyInvoiceDataManager>();
            thirdPartyInvoiceKey = 12345;
            command = new EscalateThirdPartyInvoiceForApprovalCommand(thirdPartyInvoiceKey, uosKey);
            handler = new EscalateThirdPartyInvoiceForApprovalCommandHandler(dataManager, eventRaiser);
            serviceRequestMetaData.WhenToldTo(x => x.UserName).Return(@"SAHL\PaymentProcessor");
            serviceRequestMetaData.WhenToldTo(x => x.UserOrganisationStructureKey).Return(1);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_use_the_data_manager_to_change_the_status_to_captured = () =>
        {
            dataManager.WasToldTo(x => x.UpdateThirdPartyInvoiceStatus(thirdPartyInvoiceKey, Core.BusinessModel.Enums.InvoiceStatus.AwaitingApproval));
        };

        private It should_raise_an_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Arg.Any<DateTime>(), Arg.Is<ThirdPartyInvoiceEscalatedForApprovalEvent>(y => y.ThirdPartyInvoiceKey == command.ThirdPartyInvoiceKey
              && y.EscalatedBy == serviceRequestMetaData.UserOrganisationStructureKey && y.EscalatedTo == command.UOSKeyForEscalatedUser),
              command.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, serviceRequestMetaData));
        };
    }
}