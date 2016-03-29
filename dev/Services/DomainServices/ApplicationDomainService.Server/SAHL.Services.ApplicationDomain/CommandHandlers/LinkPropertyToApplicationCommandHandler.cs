
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using System;
namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class LinkPropertyToApplicationCommandHandler : IDomainServiceCommandHandler<LinkPropertyToApplicationCommand, PropertyLinkedToApplicationEvent>
    {
        private IApplicationDataManager applicationDataManager;
        private PropertyLinkedToApplicationEvent @event;
        private IUnitOfWorkFactory uowFactory;
        private IEventRaiser eventRaiser;

        public LinkPropertyToApplicationCommandHandler(IApplicationDataManager applicationDataManager, IUnitOfWorkFactory uowFactory, IEventRaiser eventRaiser)
        {
            this.applicationDataManager = applicationDataManager;
            this.uowFactory = uowFactory;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(LinkPropertyToApplicationCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();

            applicationDataManager.LinkPropertyToApplication(command.ApplicationNumber, command.PropertyKey);

            @event = new PropertyLinkedToApplicationEvent(command.ApplicationNumber, command.PropertyKey, DateTime.Now);
            eventRaiser.RaiseEvent(@event.Date, @event, command.ApplicationNumber, (int)GenericKeyType.Property, metadata);

            return systemMessages;
        }
    }
}
