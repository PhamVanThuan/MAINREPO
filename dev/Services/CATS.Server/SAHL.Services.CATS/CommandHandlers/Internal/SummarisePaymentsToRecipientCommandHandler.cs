using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Events;
using System;
using System.Linq;

namespace SAHL.Services.CATS.CommandHandlers.Internal
{
    public class SummarisePaymentsToRecipientCommandHandler : IServiceCommandHandler<SummarisePaymentsToRecipientCommand>
    {
        private IEventRaiser eventRaiser;

        public SummarisePaymentsToRecipientCommandHandler(IEventRaiser eventRaiser)
        {
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(SummarisePaymentsToRecipientCommand command, IServiceRequestMetadata metadata)
        {
            var @event = new SummarisedPaymentsToRecipientEvent(DateTime.Now, command.PaymentsCollection.First().EmailAddress, command.PaymentsCollection);
            eventRaiser.RaiseEvent(DateTime.Now, @event, command.ClientKey, (int)GenericKeyType.LegalEntity, metadata);

            return SystemMessageCollection.Empty();
        }
    }
}
