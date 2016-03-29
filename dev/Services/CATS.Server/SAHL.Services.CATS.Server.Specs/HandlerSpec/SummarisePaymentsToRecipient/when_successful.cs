using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.CommandHandlers.Internal;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.CATS.Server.Specs.HandlerSpec.SummarisePaymentsToRecipient
{
    public class when_successful : WithFakes
    {
        private static IEventRaiser eventRaiser;
        private static SummarisePaymentsToRecipientCommand command;
        private static SummarisePaymentsToRecipientCommandHandler handler;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;
        private static string targetEmailAddress;
        private static IEnumerable<CATSPaymentBatchItemDataModel> invoicePayments;
        private static int legalEntityKey, invoiceKey, accountKey;
        private static decimal invoiceTotal;

        Establish context = () =>
        {
            eventRaiser = An<IEventRaiser>();
            metadata = An<IServiceRequestMetadata>();
            targetEmailAddress = "payments@straussdaly.co.za";

            legalEntityKey = 2005;
            invoiceKey = 147;
            accountKey = 1005;
            invoiceTotal = 200m;

            invoicePayments = new List<CATSPaymentBatchItemDataModel>
            {
                new CATSPaymentBatchItemDataModel(invoiceKey, (int) GenericKeyType.ThirdPartyInvoice, accountKey, invoiceTotal, 
                    100, 1001, 11110, "SAHL", "SAHL      SPV 32", "Strauss Dally", "Eternal Reference",
                    targetEmailAddress, legalEntityKey, true)
            };

            command = new SummarisePaymentsToRecipientCommand(legalEntityKey, invoicePayments);
            handler = new SummarisePaymentsToRecipientCommandHandler(eventRaiser);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        It should_raise_an_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                    Param<SummarisedPaymentsToRecipientEvent>.Matches(y =>
                        y.EmailAddress == targetEmailAddress
                        && y.InvoicePayments == invoicePayments
                    ), command.ClientKey, (int)GenericKeyType.LegalEntity, metadata
                ));
        };

        It should_not_return_any_messages = () =>
        {
            messages.AllMessages.Count().ShouldEqual(0);
        };

    }
}
