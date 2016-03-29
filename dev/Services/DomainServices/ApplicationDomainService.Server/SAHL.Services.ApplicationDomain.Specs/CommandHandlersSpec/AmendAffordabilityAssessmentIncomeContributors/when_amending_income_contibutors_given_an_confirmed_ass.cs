using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AmendAffordabilityAssessmentIncomeContributors
{
    public class when_amending_income_contibutors_given_an_confirmed_ass : WithCoreFakes
    {
        private static AmendAffordabilityAssessmentIncomeContributorsCommand command;
        private static AmendAffordabilityAssessmentIncomeContributorsCommandHandler handler;
        private static AffordabilityAssessmentModel affordabilityAssessmentModel;
        private static IDomainRuleManager<AffordabilityAssessmentModel> domainRuleManager;

        private Establish context = () =>
        {
            affordabilityAssessmentModel = new AffordabilityAssessmentModel(0, 0, AffordabilityAssessmentStatus.Confirmed, DateTime.Now, 0, 0, 0, new List<int>(), null, null);
            domainRuleManager = An<IDomainRuleManager<AffordabilityAssessmentModel>>();

            command = new AmendAffordabilityAssessmentIncomeContributorsCommand(affordabilityAssessmentModel);
            handler = new AmendAffordabilityAssessmentIncomeContributorsCommandHandler(domainRuleManager, serviceCommandRouter, eventRaiser);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_call_the_copy_command = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param.IsAny<CopyAffordabilityAssessmentIncomeContributorsCommand>(), serviceRequestMetaData));
        };

        private It should_raise_an_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                                                    Param.IsAny<AffordabilityAssessmentIncomeContributorsAmendedEvent>(),
                                                    Param.IsAny<int>(),
                                                    (int)GenericKeyType.AffordabilityAssessment,
                                                    Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}