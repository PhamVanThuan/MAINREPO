using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Factories;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.X2ResponseFactorySpecs
{
    public class when_creating_a_success_response : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2ResponseFactory> autoMocker;
        private static X2Response response;
        private static X2Request request;
        private static Guid guid;
        private static long instanceID;
        private static IServiceRequestMetadata serviceRequestMetadata;

        Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2ResponseFactory>();
            instanceID = 123465789L;
            guid = Guid.NewGuid();
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2RequestForExistingInstance(guid, 1, X2RequestType.UserStart, serviceRequestMetadata, "activityName", false);
        };

        Because of = () =>
        {
            response = autoMocker.ClassUnderTest.CreateSuccessResponse(request, instanceID, new SystemMessageCollection());
        };

        It should_return_a_response_with_the_guid_from_the_request = () =>
        {
            response.RequestID.ShouldEqual(guid);
        };

        It should_return_a_response_with_an_empty_error_message = () =>
        {
            response.Message.ShouldEqual(string.Empty);
        };

        It should_return_a_response_with_the_correct_instanceID = () =>
            {
                response.InstanceId.ShouldEqual(instanceID);
            };
    }
}