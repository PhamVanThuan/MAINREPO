using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessmentData
{
    public class when_inserting_affordability_assessment_legalentity : WithFakes
    {
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static FakeDbFactory fakedDb;

        private static AffordabilityAssessmentLegalEntityDataModel dataModel;

        private Establish context = () =>
        {
            dataModel = new AffordabilityAssessmentLegalEntityDataModel(43434, 565656);
            
            fakedDb = new FakeDbFactory();
            affordabilityAssessmentDataManager = new AffordabilityAssessmentDataManager(fakedDb);
        };

        private Because of = () =>
        {
            affordabilityAssessmentDataManager.InsertAffordabilityAssessmentLegalEntity(dataModel);
        };

        private It should_insert_the_affordability_assessment = () =>
        {
            fakedDb.FakedDb.DbContext.WasToldTo(x => x.Insert<AffordabilityAssessmentLegalEntityDataModel>(dataModel));
        };
    }
}
