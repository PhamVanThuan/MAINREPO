using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using System;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.DeleteUnconfirmedAffordabilityAssessment
{
    public class when_deleting_an_unconfirmed_affordability_assessment : WithCoreFakes
    {
        private static DeleteUnconfirmedAffordabilityAssessmentCommand command;
        private static DeleteUnconfirmedAffordabilityAssessmentCommandHandler handler;
        private static int affordabilityAssessmentKey;
        private static IAffordabilityAssessmentManager affordabilityAssessmentManager;

        private Establish context = () =>
        {
            affordabilityAssessmentKey = 0;

            affordabilityAssessmentManager = An<IAffordabilityAssessmentManager>();

            command = new DeleteUnconfirmedAffordabilityAssessmentCommand(affordabilityAssessmentKey);
            handler = new DeleteUnconfirmedAffordabilityAssessmentCommandHandler(affordabilityAssessmentManager, eventRaiser);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_delete_the_assesment = () =>
        {
            affordabilityAssessmentManager.WasToldTo(x => x.DeleteUnconfirmedAffordabilityAssessment(affordabilityAssessmentKey));
        };

        private It should_raise_an_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), 
                                                    Param.IsAny<AffordabilityAssessmentDeletedEvent>(),
                                                    affordabilityAssessmentKey, 
                                                    (int)GenericKeyType.AffordabilityAssessment, 
                                                    Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}