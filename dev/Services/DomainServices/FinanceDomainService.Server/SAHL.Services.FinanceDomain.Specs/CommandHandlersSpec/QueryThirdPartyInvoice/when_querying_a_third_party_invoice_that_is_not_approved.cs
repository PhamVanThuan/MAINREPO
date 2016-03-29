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
    public class when_querying_a_third_party_invoice_that_is_not_approved : WithCoreFakes
    {
        private static QueryThirdPartyInvoiceCommand command;
        public static int thirdPartyInvoiceKey { get; private set; }
        private static QueryThirdPartyInvoiceCommandHandler handler;
        private static IThirdPartyInvoiceDataManager dataManager;
        private static string queryComments;

        private Establish context = () =>
        {
            dataManager = An<IThirdPartyInvoiceDataManager>();
            thirdPartyInvoiceKey = 12345;
            queryComments = "Query Comments";
            command = new QueryThirdPartyInvoiceCommand(thirdPartyInvoiceKey, queryComments);
            handler = new QueryThirdPartyInvoiceCommandHandler(dataManager, eventRaiser);
            serviceRequestMetaData.WhenToldTo(x => x.UserName).Return(@"SAHL\PaymentProcessor");
            dataManager.WhenToldTo(x => x.HasThirdPartyInvoiceBeenApproved(thirdPartyInvoiceKey)).Return(false);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_confirm_that_invoice_is_not_approved = () =>
        {
            dataManager.WasToldTo(x => x.HasThirdPartyInvoiceBeenApproved(thirdPartyInvoiceKey));
        };

        private It should_use_the_data_manager_to_change_the_status_to_captured = () =>
        {
            dataManager.WasToldTo(x => x.UpdateThirdPartyInvoiceStatus(thirdPartyInvoiceKey, Core.BusinessModel.Enums.InvoiceStatus.Captured));
        };

        private It should_raise_an_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Arg.Any<DateTime>(), Arg.Is<ThirdPartyInvoiceQueriedPreApprovalEvent>(y => y.ThirdPartyInvoiceKey == command.ThirdPartyInvoiceKey
              && y.QueryInitiatedBy == serviceRequestMetaData.UserName && y.QueryComments == command.QueryComments),
              command.ThirdPartyInvoiceKey, (int)GenericKeyType.ThirdPartyInvoice, serviceRequestMetaData));
        };
    }
}