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
    public class when_told_to_get_affordability_assessment_stress_factors : WithFakes
    {
        private static AutoMocker<ApplicationDomainService> autoMocker = new NSubstituteAutoMocker<ApplicationDomainService>();
        private static IEnumerable<AffordabilityAssessmentStressFactorModel> result;
        private static int expectedResult = 4;

        Establish context = () =>
        {
            var serviceQueryResult = new ServiceQueryResult<AffordabilityAssessmentStressFactorModel>(new AffordabilityAssessmentStressFactorModel[]
                {
                    new AffordabilityAssessmentStressFactorModel(1, "0%", 0.00),
                    new AffordabilityAssessmentStressFactorModel(2, "0.5%", 0.04),
                    new AffordabilityAssessmentStressFactorModel(3, "1.0%", 0.07),
                    new AffordabilityAssessmentStressFactorModel(4, "2.0%", 0.14)

                });

            autoMocker.Get<IApplicationDomainServiceClient>().WhenToldTo(x => x.PerformQuery(Param.IsAny<GetAffordabilityAssessmentStressFactorsQuery>())).Return(new SystemMessageCollection());

            autoMocker.Get<IApplicationDomainServiceClient>().WhenToldTo<IApplicationDomainServiceClient>(x => x.PerformQuery(Param.IsAny<GetAffordabilityAssessmentStressFactorsQuery>())).Callback<GetAffordabilityAssessmentStressFactorsQuery>(y => { y.Result = serviceQueryResult; });
        };

        Because of = () =>
        {
            result = autoMocker.ClassUnderTest.GetAffordabilityAssessmentStressFactors();
        };

        It should_return_a_successful_result = () =>
        {
            result.Count().ShouldEqual(expectedResult);
        };

    }
}
