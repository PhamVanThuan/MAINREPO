using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using SAHL.V3.Framework.DomainServices;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Specs.DomainServicesSpecs.ApplicationDomainServiceSpecs
{
    public class when_told_to_get_historical_affordability_assessments_for_application : WithFakes
    {
        private static AutoMocker<ApplicationDomainService> autoMocker = new NSubstituteAutoMocker<ApplicationDomainService>();
        private static IEnumerable<AffordabilityAssessmentSummaryModel> result;
        private static int expectedResult = 1;

        Establish context = () =>
        {
            var serviceQueryResult = new ServiceQueryResult<AffordabilityAssessmentSummaryModel>(new AffordabilityAssessmentSummaryModel[]
                {
                    new AffordabilityAssessmentSummaryModel()

                });

            autoMocker.Get<IApplicationDomainServiceClient>().WhenToldTo(x => x.PerformQuery(Param.IsAny<GetHistoricalAffordabilityAssessmentsForApplicationQuery>())).Return(new SystemMessageCollection());

            autoMocker.Get<IApplicationDomainServiceClient>().WhenToldTo<IApplicationDomainServiceClient>(x => x.PerformQuery(Param.IsAny<GetHistoricalAffordabilityAssessmentsForApplicationQuery>())).Callback<GetHistoricalAffordabilityAssessmentsForApplicationQuery>(y => { y.Result = serviceQueryResult; });
        };

        Because of = () =>
        {
            result = autoMocker.ClassUnderTest.GetHistoricalAffordabilityAssessmentsForApplication(Param.IsAny<int>());
        };

        It should_return_a_successful_result = () =>
        {
            result.Count().ShouldEqual(expectedResult);
        };

    }
}
