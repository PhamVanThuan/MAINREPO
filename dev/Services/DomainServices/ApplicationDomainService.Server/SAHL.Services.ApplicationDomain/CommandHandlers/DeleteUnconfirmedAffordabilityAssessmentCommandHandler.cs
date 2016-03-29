using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class DeleteUnconfirmedAffordabilityAssessmentCommandHandler 
        : IDomainServiceCommandHandler<DeleteUnconfirmedAffordabilityAssessmentCommand, AffordabilityAssessmentDeletedEvent>
    {
        private IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private IEventRaiser eventRaiser;

        public DeleteUnconfirmedAffordabilityAssessmentCommandHandler(IAffordabilityAssessmentManager affordabilityAssessmentManager, IEventRaiser eventRaiser)
        {
            this.affordabilityAssessmentManager = affordabilityAssessmentManager;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(DeleteUnconfirmedAffordabilityAssessmentCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();

            AffordabilityAssessmentModel affordabilityAssessment = affordabilityAssessmentManager.GetAffordabilityAssessment(command.AffordabilityAssessmentKey);
            affordabilityAssessmentManager.DeleteUnconfirmedAffordabilityAssessment(command.AffordabilityAssessmentKey);

            eventRaiser.RaiseEvent(DateTime.Now, 
                    new AffordabilityAssessmentDeletedEvent(DateTime.Now, 
                                                            affordabilityAssessment), 
                                                            command.AffordabilityAssessmentKey, 
                                                            (int)GenericKeyType.AffordabilityAssessment, 
                                                            metadata);

            return systemMessageCollection;
        }
    }
}