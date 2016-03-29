using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.CLR;
using StructureMap.AutoMocking;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.CLR.Specs
{
    public class when_sending_an_external_activity_web_request : WithFakes
    {
        private static AutoMocker<UserDefinedFunctions> autoMocker;

        private static X2ExternalActivityRequest request;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<UserDefinedFunctions>();
            var serviceRequestMetadata = new ServiceRequestMetadata();
            request = new X2ExternalActivityRequest(Guid.NewGuid(), 0, 1, 2, 3, 4, serviceRequestMetadata);
        };

        private Because of = () =>
        {
            //autoMocker.ClassUnderTest.SendExternalActivityWebRequest(1);
        };

        private It should_forward_the_request_to_the_engine = () =>
        {
            //autoMocker.Get<UserDefinedFunctions>().WasToldTo(x => x.SendExternalActivityWebRequest(1));
        };
    }
}
