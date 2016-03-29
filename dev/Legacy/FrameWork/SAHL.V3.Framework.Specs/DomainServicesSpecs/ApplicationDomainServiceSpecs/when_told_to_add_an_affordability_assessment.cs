using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.V3.Framework.DomainServices;

namespace SAHL.V3.Framework.Specs.DomainServicesSpecs.ApplicationDomainServiceSpecs
{
    public class when_told_to_add_an_affordability_assessment : WithFakes
    {
        private static IApplicationDomainService applicationDomainService;
        private static bool actualResult;
        private static bool expectedResult = true;

        private Establish context = () =>
        {
            IApplicationDomainServiceClient client = An<IApplicationDomainServiceClient>();
            IV3ServiceCommon v3ServiceCommon = An<IV3ServiceCommon>();
            client.WhenToldTo(x => x.PerformCommand(Param.IsAny<IApplicationDomainCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(new SystemMessageCollection());
            applicationDomainService = new ApplicationDomainService(client, v3ServiceCommon);
        };

        private Because of = () =>
        {
            actualResult = applicationDomainService.AddAffordabilityAssessment(Param.IsAny<AffordabilityAssessmentModel>());
        };

        private It should_return_a_successful_result = () =>
        {
            actualResult.ShouldEqual(expectedResult);
        };
    }
}