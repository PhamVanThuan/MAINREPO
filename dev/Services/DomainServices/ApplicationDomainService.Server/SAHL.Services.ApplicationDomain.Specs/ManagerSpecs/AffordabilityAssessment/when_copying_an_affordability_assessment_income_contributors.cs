using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessment
{
    public class when_copying_an_affordability_assessment_income_contributors : WithFakes
    {
        private static IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static AffordabilityAssessmentModel affordabilityAssessment;
        private static AffordabilityAssessmentDataModel affordabilityAssessmentDataModel;
        private static IEnumerable<AffordabilityAssessmentItemDataModel> affordabilityAssessmentItems;

        private Establish context = () =>
        {
            affordabilityAssessmentDataManager = An<IAffordabilityAssessmentDataManager>();
            affordabilityAssessmentManager = new AffordabilityAssessmentManager(affordabilityAssessmentDataManager);

            affordabilityAssessmentDataModel = new AffordabilityAssessmentDataModel(0, 0, 0, 0, 0, 0, DateTime.Now, 0, 0, 0, null, null, null);
            affordabilityAssessmentDataManager.WhenToldTo(x => x.GetAffordabilityAssessmentByKey(Param.IsAny<int>())).Return(affordabilityAssessmentDataModel);

            affordabilityAssessmentItems = new List<AffordabilityAssessmentItemDataModel>()
            {
                new AffordabilityAssessmentItemDataModel(1,2,3,DateTime.Now,5,555,5555,5555, ""),
                new AffordabilityAssessmentItemDataModel(3,3,363,DateTime.Now,335,33555,335555,3333355, "")
            };
            affordabilityAssessmentDataManager.WhenToldTo(x => x.GetAffordabilityAssessmentItems(Param.IsAny<int>())).Return(affordabilityAssessmentItems);

            affordabilityAssessment = new AffordabilityAssessmentModel(0,
                                                                        0,
                                                                        AffordabilityAssessmentStatus.Unconfirmed,
                                                                        DateTime.Now,
                                                                        0,
                                                                        0,
                                                                        0,
                                                                        new List<int>() { 1, 2 },
                                                                        new AffordabilityAssessmentDetailModel(), null);
        };

        private Because of = () =>
        {
            affordabilityAssessmentManager.CopyAndArchiveAffordabilityAssessmentWithNewIncomeContributors(affordabilityAssessment, 0);
        };

        private It should_update_the_assessment = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(x => x.UpdateAffordabilityAssessment(Param.IsAny<AffordabilityAssessmentDataModel>()));
        };

        private It should_insert_items = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(x => x.InsertAffordabilityAssessmentItem(Param.IsAny<AffordabilityAssessmentItemDataModel>()));
        };

        private It should_insert_income_contributors = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(x => x.InsertAffordabilityAssessmentLegalEntity(Param.IsAny<AffordabilityAssessmentLegalEntityDataModel>()));
        };
    }
}