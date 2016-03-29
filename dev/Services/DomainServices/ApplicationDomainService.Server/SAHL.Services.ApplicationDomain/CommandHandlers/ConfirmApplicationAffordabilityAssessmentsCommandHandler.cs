using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class ConfirmApplicationAffordabilityAssessmentsCommandHandler
        : IDomainServiceCommandHandler<ConfirmApplicationAffordabilityAssessmentsCommand, ApplicationAffordabilityAssessmentsConfirmedEvent>
    {
        private IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private IEventRaiser eventRaiser;

        public ConfirmApplicationAffordabilityAssessmentsCommandHandler(IAffordabilityAssessmentManager affordabilityAssessmentManager, IEventRaiser eventRaiser)
        {
            this.affordabilityAssessmentManager = affordabilityAssessmentManager;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(ConfirmApplicationAffordabilityAssessmentsCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();

            IEnumerable<AffordabilityAssessmentModel> unconfirmedAffordabilityAssessments = 
                                affordabilityAssessmentManager.GetApplicationAffordabilityAssessments(command.ApplicationKey)
                                            .Where(x => x.AffordabilityAssessmentStatus == AffordabilityAssessmentStatus.Unconfirmed);

            affordabilityAssessmentManager.ConfirmAffordabilityAssessments(unconfirmedAffordabilityAssessments);

            DateTime now = DateTime.Now;
            eventRaiser.RaiseEvent(now, new ApplicationAffordabilityAssessmentsConfirmedEvent(now, unconfirmedAffordabilityAssessments), command.ApplicationKey, (int)GenericKeyType.Offer, metadata);

            return systemMessageCollection;
        }
    }
}