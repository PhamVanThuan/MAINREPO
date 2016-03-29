using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessment
{
    public class when_confirming_an_unconfirmed_affordability_assessment : WithFakes
    {
        private static IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;

        private static IEnumerable<AffordabilityAssessmentModel> affordabilityAssessmentModels;
        private static int affordabilityAssessmentKey;

        private Establish context = () =>
        {
            affordabilityAssessmentKey = 1;

            affordabilityAssessmentDataManager = An<IAffordabilityAssessmentDataManager>();

            affordabilityAssessmentModels = new List<AffordabilityAssessmentModel>()
            {
                new AffordabilityAssessmentModel(affordabilityAssessmentKey,
                                                1,AffordabilityAssessmentStatus.Unconfirmed,
                                                DateTime.Now,
                                                1,
                                                1,
                                                1,
                                                new List<int>() {1, 2},
                                                new AffordabilityAssessmentDetailModel(), null)
            };

            affordabilityAssessmentManager = new AffordabilityAssessmentManager(affordabilityAssessmentDataManager);
        };

        private Because of = () =>
        {
            affordabilityAssessmentManager.ConfirmAffordabilityAssessments(affordabilityAssessmentModels);
        };

        private It should_tell_the_affordability_assessment_data_manager_to_confirm_the_affordability_assessment_status_to_confirmed = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(x => x.ConfirmAffordabilityAssessment(affordabilityAssessmentKey));
        };
    }
}