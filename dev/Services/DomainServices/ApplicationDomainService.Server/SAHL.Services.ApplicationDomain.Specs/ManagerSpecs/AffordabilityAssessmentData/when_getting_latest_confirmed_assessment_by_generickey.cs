using Machine.Specifications;
using Machine.Fakes;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessmentData
{
    public class when_getting_latest_confirmed_assessment_by_generickey
    {
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static FakeDbFactory fakedDb;

        private static int genericKey;
        private static int genericKeyType;

        private Establish context = () =>
        {
            genericKey = 1;
            genericKeyType = 9;

            fakedDb = new FakeDbFactory();
            affordabilityAssessmentDataManager = new AffordabilityAssessmentDataManager(fakedDb);
        };

        private Because of = () =>
        {
            affordabilityAssessmentDataManager.GetLatestConfirmedAffordabilityAssessmentByGenericKey(genericKey, genericKeyType);
        };

        private It should_execute_the_get_the_latest_confirmed_affordability_assessment_by_generickey_statement = () =>
        {
            fakedDb.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne<AffordabilityAssessmentDataModel>(
                Arg.Is<GetLatestConfirmedAffordabilityAssessmentByGenericKeyStatement>(y =>
                    y.GenericKey == genericKey &&
                    y.GenericKeyTypeKey == genericKeyType)));
        };
    }
}
