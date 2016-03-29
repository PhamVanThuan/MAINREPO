using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class SetExternalOriginatorAttributeCommandHandler : IDomainServiceCommandHandler<SetExternalOriginatorAttributeCommand, ExternalOriginatorAttributeSetEvent>
    {
        private IApplicationDataManager applicationDataManager;
        private IEventRaiser eventRaiser;
        private IUnitOfWorkFactory uowFactory;

        public SetExternalOriginatorAttributeCommandHandler(IApplicationDataManager applicationDataManager, 
            IEventRaiser eventRaiser, IUnitOfWorkFactory uowFactory)
        {
            this.applicationDataManager = applicationDataManager;
            this.eventRaiser = eventRaiser;
            this.uowFactory = uowFactory;
        }

        public ISystemMessageCollection HandleCommand(SetExternalOriginatorAttributeCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            // do validation
            if (command.OriginationSource != OriginationSource.Capitec && command.OriginationSource != OriginationSource.Comcorp)
            {
                messages.AddMessage(new SystemMessage("The originator attribute can only be set for Comcorp or Capitec originated applications", 
                    SystemMessageSeverityEnum.Error));
                return messages;
            }

            OfferAttributeType attributeType = OfferAttributeType.CapitecLoan;
            // save the offer attribute if no errors
            if (!messages.HasErrors)
            {
                using (var uow = uowFactory.Build())
                {
                    switch (command.OriginationSource)
                    {
                        case OriginationSource.Comcorp:
                            attributeType = OfferAttributeType.ComcorpLoan;
                            break;

                        case OriginationSource.Capitec:
                            attributeType = OfferAttributeType.CapitecLoan;
                            break;
                        default:
                            break;
                    }
                    OfferAttributeDataModel offerAttributeDataModel = new OfferAttributeDataModel(command.ApplicationNumber, (int)attributeType);
                    applicationDataManager.SaveExternalOriginatorAttribute(offerAttributeDataModel);
                    eventRaiser.RaiseEvent(DateTime.Now, new ExternalOriginatorAttributeSetEvent(DateTime.Now, (int)command.OriginationSource, command.ApplicationNumber, attributeType), 
                        command.ApplicationNumber, (int)GenericKeyType.Offer, metadata);

                    uow.Complete();
                }
            }
            return messages;
        }
    }
}