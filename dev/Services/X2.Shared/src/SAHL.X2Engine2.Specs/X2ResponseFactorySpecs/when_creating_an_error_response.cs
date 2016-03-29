using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Factories;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.X2ResponseFactorySpecs
{
    public class when_creating_an_error_response : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2ResponseFactory> autoMocker;

        private static X2Response response;
        private static X2Request request;
        private static string errorMessage;
        private static Guid guid;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2ResponseFactory>();

            guid = Guid.NewGuid();
            errorMessage = "ErrorMessage";
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2RequestForExistingInstance(guid, 1, X2RequestType.UserStart, serviceRequestMetadata, "activityName", false);
        };

        private Because of = () =>
        {
            response = autoMocker.ClassUnderTest.CreateErrorResponse(request, errorMessage, 0, Param.IsAny<SystemMessageCollection>());
        };

        private It should_return_an_error_response_with_the_correct_message = () =>
        {
            response.RequestID.ShouldEqual(guid);
        };

        private It should_return_an_error_response_with_the_correct_guid = () =>
        {
            response.Message.ShouldEqual(errorMessage);
        };
    }
}