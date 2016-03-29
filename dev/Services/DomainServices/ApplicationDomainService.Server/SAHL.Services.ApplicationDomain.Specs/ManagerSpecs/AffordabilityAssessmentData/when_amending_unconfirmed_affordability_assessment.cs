using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessmentData
{
    public class when_amending_unconfirmed_affordability_assessment : WithFakes
    {
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static FakeDbFactory fakedDb;

        private Establish context = () =>
        {
            fakedDb = new FakeDbFactory();
            affordabilityAssessmentDataManager = new AffordabilityAssessmentDataManager(fakedDb);
        };

        private Because of = () =>
        {
            affordabilityAssessmentDataManager.UpdateAffordabilityAssessmentItem(Param.IsAny<AffordabilityAssessmentItemDataModel>());
        };

        private It should_update_the_affordability_assessment_items = () =>
        {
            fakedDb.FakedDb.DbContext.WasToldTo(x => x.Update<AffordabilityAssessmentItemDataModel>(Param.IsAny<AffordabilityAssessmentItemDataModel>()));
        };
    }
}