using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.RejectThirdPartyInvoice
{
    public class when_rejecting_a_thirdparty_invoice_after_approval : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static RejectThirdPartyInvoiceCommandHandler commandHandler;
        private static RejectThirdPartyInvoiceCommand command;
        private static string emailSubject;
        private static string rejectedBy;
        private static ThirdPartyInvoiceDataModel invoice;

        private Establish context = () =>
        {
            invoice = new ThirdPartyInvoiceDataModel(2, "ref", 3, 12345, Guid.NewGuid(), "098876", DateTime.Now, "jane@doe.com", 10, 1, 12, true, DateTime.Now, string.Empty);
            emailSubject = "this is a subject";
            rejectedBy = "Bob";
            command = new RejectThirdPartyInvoiceCommand(invoice.ThirdPartyInvoiceKey, "this is a comment");
            serviceRequestMetaData.WhenToldTo(x => x.UserName).Return(rejectedBy);
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            thirdPartyInvoiceDataManager.WhenToldTo(x => x.HasThirdPartyInvoiceBeenApproved(command.ThirdPartyInvoiceKey)).Return(true);
            thirdPartyInvoiceDataManager.WhenToldTo(x => x.GetThirdPartyInvoiceEmailSubject(command.ThirdPartyInvoiceKey))
                .Return(emailSubject);
            thirdPartyInvoiceDataManager.WhenToldTo(x => x.GetThirdPartyInvoiceByKey(command.ThirdPartyInvoiceKey))
                .Return(invoice);
            commandHandler = new RejectThirdPartyInvoiceCommandHandler(eventRaiser, thirdPartyInvoiceDataManager);
        };

        private Because of = () =>
        {
            commandHandler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_invoice_has_been_rejected = () =>
        {
            thirdPartyInvoiceDataManager.WasToldTo(x => x.HasThirdPartyInvoiceBeenApproved(command.ThirdPartyInvoiceKey));
        };

        private It should_change_invoice_status_to_rejected = () =>
        {
            thirdPartyInvoiceDataManager.WasToldTo(x => x.UpdateThirdPartyInvoiceStatus(command.ThirdPartyInvoiceKey, InvoiceStatus.Rejected));
        };

        private It should_raise_a_third_party_invoice_rejected_post_approval_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                    Param<ThirdPartyInvoiceRejectedPostApprovalEvent>.Matches(y => y.AccountNumber == invoice.AccountKey &&
                                                                    y.AttorneyEmailAddress == invoice.ReceivedFromEmailAddress &&
                                                                    y.InvoiceNumber == invoice.InvoiceNumber &&
                                                                    y.RejectedBy == rejectedBy &&
                                                                    y.RejectionComments == command.RejectionComments &&
                                                                    y.SAHLReferenceNumber == invoice.SahlReference &&
                                                                    y.ThirdPartyInvoiceKey == invoice.ThirdPartyInvoiceKey &&
                                                                    y.EmailSubject == emailSubject),
                invoice.ThirdPartyInvoiceKey,
                (int)GenericKeyType.ThirdPartyInvoice, Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}