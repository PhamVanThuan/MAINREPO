using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessmentData
{
    public class when_deleteing_unconfirmed_affordability_assessment : WithFakes
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
            affordabilityAssessmentDataManager.DeleteUnconfirmedAffordabilityAssessment(Param.IsAny<int>());
        };

        private It should_delete_all_linked_affordability_assessment_legal_entities = () =>
        {
            fakedDb.FakedDb.DbContext.WasToldTo(x => x.DeleteWhere<AffordabilityAssessmentLegalEntityDataModel>(Param.IsAny<string>(), Param.IsAny<object>()));
        };

        private It should_delete_all_linked_affordability_assessment_items = () =>
        {
            fakedDb.FakedDb.DbContext.WasToldTo(x => x.DeleteWhere<AffordabilityAssessmentItemDataModel>(Param.IsAny<string>(), Param.IsAny<object>()));
        };

        private It should_delete_the_affordability_assessment = () =>
        {
            fakedDb.FakedDb.DbContext.WasToldTo(x => x.DeleteByKey<AffordabilityAssessmentDataModel, int>(Param.IsAny<int>()));
        };
    }
}