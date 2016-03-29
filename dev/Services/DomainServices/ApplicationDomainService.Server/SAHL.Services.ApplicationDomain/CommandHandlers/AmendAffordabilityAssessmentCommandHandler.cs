using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using System;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class AmendAffordabilityAssessmentCommandHandler : IDomainServiceCommandHandler<AmendAffordabilityAssessmentCommand, AffordabilityAssessmentAmendedEvent>
    {
        private IServiceCommandRouter serviceCommandRouter;
        private IEventRaiser eventRaiser;

        public AmendAffordabilityAssessmentCommandHandler(IServiceCommandRouter serviceCommandRouter, IEventRaiser eventRaiser)
        {
            this.serviceCommandRouter = serviceCommandRouter;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(AmendAffordabilityAssessmentCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();

            if (command.AffordabilityAssessment.AffordabilityAssessmentStatus == AffordabilityAssessmentStatus.Confirmed)
            {
                CopyAffordabilityAssessmentCommand copyAffordabilityAssessmentCommand = new CopyAffordabilityAssessmentCommand(command.AffordabilityAssessment);
                serviceCommandRouter.HandleCommand<CopyAffordabilityAssessmentCommand>(copyAffordabilityAssessmentCommand, metadata);
            }
            else
            {
                UpdateAffordabilityAssessmentCommand updateAffordabilityAssessmentCommand = new UpdateAffordabilityAssessmentCommand(command.AffordabilityAssessment);
                serviceCommandRouter.HandleCommand<UpdateAffordabilityAssessmentCommand>(updateAffordabilityAssessmentCommand, metadata);
            }

            DateTime now = DateTime.Now;
            eventRaiser.RaiseEvent(now, new AffordabilityAssessmentAmendedEvent(now, command.AffordabilityAssessment),
                                    command.AffordabilityAssessment.Key, (int)GenericKeyType.AffordabilityAssessment, metadata);

            return systemMessageCollection;
        }
    }
}