using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessmentData
{
    public class when_getting_affordability_assessment_contributors : WithFakes
    {
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static FakeDbFactory fakedDb;

        private static int affordabilityAssessmentKey;

        private Establish context = () =>
        {
            fakedDb = new FakeDbFactory();
            affordabilityAssessmentDataManager = new AffordabilityAssessmentDataManager(fakedDb);

            affordabilityAssessmentKey = 90;

            List<int> legalEntities = new List<int>();

            fakedDb.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<int>(Param.IsAny<GetAffordabilityAssessmentContributorsStatement>()))
                                             .Return(legalEntities);
        };

        private Because of = () =>
        {
            affordabilityAssessmentDataManager.GetAffordabilityAssessmentContributors(affordabilityAssessmentKey);
        };

        private It should_query_the_database_for_the_specific_stress_factor_percentage = () =>
        {
            fakedDb.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.Select<int>(Arg.Is<GetAffordabilityAssessmentContributorsStatement>(
                                                              y => y.AffordabilityAssessmentKey == affordabilityAssessmentKey)));
        };
    }
}
