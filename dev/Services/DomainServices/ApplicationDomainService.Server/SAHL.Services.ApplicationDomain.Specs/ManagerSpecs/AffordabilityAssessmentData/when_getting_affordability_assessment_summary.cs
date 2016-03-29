using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.ApplicationDomain.Managers.Application.Statements;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessmentData
{
    public class when_getting_affordability_assessment_summary : WithFakes
    {
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static FakeDbFactory fakedDb;

        private static int affordabilityAssessmentKey;

        private Establish context = () =>
        {
            affordabilityAssessmentKey = 1;

            fakedDb = new FakeDbFactory();
            affordabilityAssessmentDataManager = new AffordabilityAssessmentDataManager(fakedDb);
        };

        private Because of = () =>
        {
            affordabilityAssessmentDataManager.GetAffordabilityAssessmentSummary(affordabilityAssessmentKey);
        };

        private It should_execute_the_get_affordabilityassessment_summary_statement = () =>
        {
            fakedDb.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne<AffordabilityAssessmentSummaryModel>(
                Arg.Is<GetAffordabilityAssessmentSummaryStatement>(y =>
                    y.AffordabilityAssessmentKey == affordabilityAssessmentKey)));
        };
    }
}
