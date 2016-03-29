using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Providers;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Services;


namespace SAHL.X2Engine2.Specs.X2RequestStoreSpecs
{
    public class when_storing_a_request : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestStore> autoMocker;

        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static X2CreateInstanceRequest createInstanceRequest;
        private static RequestDataModel requestToSave;
        private static string serializeRequest;
        private static DateTime dateTime;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestStore>();

            dateTime = DateTime.Now;
            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();

            serializeRequest = Guid.NewGuid().ToString();
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            createInstanceRequest = new X2CreateInstanceRequest(Guid.NewGuid(), "activityName", "processName", "workflowName", serviceRequestMetadata, false);
            autoMocker.Get<ISerializationProvider>().WhenToldTo(x => x.Serialize<X2CreateInstanceRequest>(createInstanceRequest)).Return(serializeRequest);

            requestToSave = new RequestDataModel(createInstanceRequest.CorrelationId, serializeRequest, (int)SAHL.X2Engine2.Enumerations.RequestStatus.Received, dateTime, dateTime, 0);
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.StoreReceivedRequest(createInstanceRequest);
        };

        private It should_be_in_received_status = () =>
        {
            requestToSave.RequestStatusID.ShouldEqual((int)SAHL.X2Engine2.Enumerations.RequestStatus.Received);
        };

        private It should_serializeTheRequest = () =>
        {
            autoMocker.Get<ISerializationProvider>().WasToldTo(x => x.Serialize<X2CreateInstanceRequest>(createInstanceRequest));
        };

        private It should_store_the_request = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.Insert<RequestDataModel>(Arg.Is<RequestDataModel>(msg => msg.RequestStoreComparer(requestToSave))));
        };
    }

    public static class Extensions
    {
        public static bool RequestStoreComparer(this RequestDataModel request, RequestDataModel requestToCompare)
        {
            return request.Contents == requestToCompare.Contents &&
                request.RequestID == requestToCompare.RequestID &&
                request.RequestStatusID == requestToCompare.RequestStatusID;
        }
    }
}