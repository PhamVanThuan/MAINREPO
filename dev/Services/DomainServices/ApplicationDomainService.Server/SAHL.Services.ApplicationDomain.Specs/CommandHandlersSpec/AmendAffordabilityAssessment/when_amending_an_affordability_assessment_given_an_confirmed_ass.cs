using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AmendAffordabilityAssessment
{
    public class when_amending_an_affordability_assessment_given_an_confirmed_ass : WithCoreFakes
    {
        private static AmendAffordabilityAssessmentCommand command;
        private static AmendAffordabilityAssessmentCommandHandler handler;
        private static AffordabilityAssessmentModel affordabilityAssessmentModel;

        private Establish context = () =>
        {
            affordabilityAssessmentModel = new AffordabilityAssessmentModel(0, 0, AffordabilityAssessmentStatus.Confirmed, DateTime.Now, 0, 0, 0, new List<int>(), null, null);

            command = new AmendAffordabilityAssessmentCommand(affordabilityAssessmentModel);
            handler = new AmendAffordabilityAssessmentCommandHandler(serviceCommandRouter, eventRaiser);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_call_the_copy_command = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param.IsAny<CopyAffordabilityAssessmentCommand>(), serviceRequestMetaData));
        };

        private It should_raise_an_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                                                    Param.IsAny<AffordabilityAssessmentAmendedEvent>(),
                                                    Param.IsAny<int>(),
                                                    (int)GenericKeyType.AffordabilityAssessment,
                                                    Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}