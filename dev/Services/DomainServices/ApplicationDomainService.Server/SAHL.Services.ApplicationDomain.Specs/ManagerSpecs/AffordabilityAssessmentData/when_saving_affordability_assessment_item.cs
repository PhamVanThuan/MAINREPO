using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using System;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessmentData
{
    public class when_saving_affordability_assessment_item : WithFakes
    {
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static FakeDbFactory fakedDb;

        private static AffordabilityAssessmentItemDataModel affordabilityAssessmentItemModel;

        private Establish context = () =>
        {
            affordabilityAssessmentItemModel = new AffordabilityAssessmentItemDataModel(1, 1, 1, DateTime.Now, 1, 1, 1, 1, "");

            fakedDb = new FakeDbFactory();
            affordabilityAssessmentDataManager = new AffordabilityAssessmentDataManager(fakedDb);
        };

        private Because of = () =>
        {
            affordabilityAssessmentDataManager.InsertAffordabilityAssessmentItem(affordabilityAssessmentItemModel);
        };

        private It should_insert_the_affordability_assessment = () =>
        {
            fakedDb.FakedDb.DbContext.WasToldTo(x => x.Insert(affordabilityAssessmentItemModel));
        };
    }
}