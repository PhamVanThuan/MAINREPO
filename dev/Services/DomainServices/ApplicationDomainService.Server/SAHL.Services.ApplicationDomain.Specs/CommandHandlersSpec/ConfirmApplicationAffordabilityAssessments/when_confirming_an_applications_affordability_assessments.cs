using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.ConfirmApplicationAffordabilityAssessments
{
    public class when_confirming_an_applications_affordability_assessments : WithCoreFakes
    {
        private static ConfirmApplicationAffordabilityAssessmentsCommand command;
        private static ConfirmApplicationAffordabilityAssessmentsCommandHandler handler;

        private static int applicationKey;
        private static string userName;

        private static IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private static IEnumerable<AffordabilityAssessmentModel> affordabilityAssessmentModels;

        private static AffordabilityAssessmentModel unconfirmed1 = new AffordabilityAssessmentModel(1111, 1, AffordabilityAssessmentStatus.Unconfirmed, DateTime.Now, 1, 1, 1, 
                                                                                                        new List<int>() {1, 2}, 
                                                                                                        new AffordabilityAssessmentDetailModel(), null);

        private static AffordabilityAssessmentModel unconfirmed2 = new AffordabilityAssessmentModel(3333, 1, AffordabilityAssessmentStatus.Unconfirmed, DateTime.Now, 1, 1, 1,
                                                                                                        new List<int>() { 1, 2 },
                                                                                                        new AffordabilityAssessmentDetailModel(), null);
        private static AffordabilityAssessmentModel confirmed1 = new AffordabilityAssessmentModel(2222, 1, AffordabilityAssessmentStatus.Confirmed, DateTime.Now, 1, 1, 1,
                                                                                                        new List<int>() { 1, 2 },
                                                                                                        new AffordabilityAssessmentDetailModel(), null);



        private Establish context = () =>
        {
            applicationKey = 9999;
            userName = @"SAHL\BCUser2";

            affordabilityAssessmentModels = new List<AffordabilityAssessmentModel>()
            {
                unconfirmed1, confirmed1, unconfirmed2
            };

            affordabilityAssessmentManager = An<IAffordabilityAssessmentManager>();
            affordabilityAssessmentManager.WhenToldTo(x => x.GetApplicationAffordabilityAssessments(applicationKey)).Return(affordabilityAssessmentModels);

            serviceRequestMetaData = An<IServiceRequestMetadata>();
            serviceRequestMetaData.WhenToldTo(x => x.UserName).Return(userName);

            eventRaiser = An<IEventRaiser>();

            command = new ConfirmApplicationAffordabilityAssessmentsCommand(applicationKey);
            handler = new ConfirmApplicationAffordabilityAssessmentsCommandHandler(affordabilityAssessmentManager, eventRaiser);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_get_the_applications_assessments = () =>
        {
            affordabilityAssessmentManager.WasToldTo(x => x.GetApplicationAffordabilityAssessments(applicationKey));
        };

        private It should_only_confirm_the_unconfirmed_assessments = () =>
        {
            affordabilityAssessmentManager.WasToldTo(x => x.ConfirmAffordabilityAssessments(
                Arg.Is<IEnumerable<AffordabilityAssessmentModel>>(
                    y => y.All(z => z.AffordabilityAssessmentStatus == AffordabilityAssessmentStatus.Unconfirmed)
                )));
        };

        private It should_raise_an_event_with_only_the_assessments_that_were_confirmed = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                                                    Arg.Is<ApplicationAffordabilityAssessmentsConfirmedEvent>(
                                                        y => y.AffordabilityAssessmentsConfirmed.All(z => z.AffordabilityAssessmentStatus == AffordabilityAssessmentStatus.Unconfirmed)
                                                      ),
                                                    applicationKey,
                                                    (int)GenericKeyType.Offer,
                                                    serviceRequestMetaData));
        };
    }
}