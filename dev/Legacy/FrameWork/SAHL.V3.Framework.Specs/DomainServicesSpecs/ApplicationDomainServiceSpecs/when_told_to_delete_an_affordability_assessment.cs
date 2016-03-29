using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using SAHL.V3.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Specs.DomainServicesSpecs.ApplicationDomainServiceSpecs
{
    public class when_told_to_delete_an_affordability_assessment  : WithFakes
    {
        private static IApplicationDomainService applicationDomainService;
        private static bool actualResult;
        private static bool expectedResult = true;
        private static int affordabilityAssessmentKey = 1;

        Establish context = () =>
        {
            IApplicationDomainServiceClient client = An<IApplicationDomainServiceClient>();
            IV3ServiceCommon v3ServiceCommon = An<IV3ServiceCommon>();
            client.WhenToldTo(x => x.PerformCommand(Param.IsAny<IApplicationDomainCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(new SystemMessageCollection());
            applicationDomainService = new ApplicationDomainService(client, v3ServiceCommon);
        };

        Because of = () =>
        {
            actualResult = applicationDomainService.DeleteUnconfirmedAffordabilityAssessment(affordabilityAssessmentKey);
        };

        It should_return_a_successful_result = () =>
        {
            actualResult.ShouldEqual(expectedResult);
        };

    }
}
