using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2.Messages;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Services;


namespace SAHL.X2Engine2.Specs.X2RequestStoreSpecs
{
    public class when_storing_a_received_request_as_unserviceable_due_to_no_available_route : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestStore> autoMocker;
        private static IX2Request createInstanceRequest;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static RequestDataModel requestFromDatabase;
        private static DateTime todaysDate;
        private static int timeoutRetriesCount;
        private static IServiceRequestMetadata serviceRequestMetadata;

        Establish context = () =>
        {
            timeoutRetriesCount = 0;
            todaysDate = DateTime.Now;
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestStore>();
            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            createInstanceRequest = new X2CreateInstanceRequest(Guid.NewGuid(), "activityName", "processName", "workflowName", serviceRequestMetadata, false);
            requestFromDatabase = new RequestDataModel(createInstanceRequest.CorrelationId, Guid.NewGuid().ToString(), 1, todaysDate, todaysDate, timeoutRetriesCount);
            readWriteSqlRepository.WhenToldTo(x => x.SelectOne<RequestDataModel>(Param.IsAny<string>(), Param.IsAny<object>())).Return(requestFromDatabase);
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.StoreReceivedRequestAsUnserviceableDueToNoAvailableRoute(createInstanceRequest);
        };

        It should_get_the_request = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.SelectOne<RequestDataModel>(Param.IsAny<string>(), Param.IsAny<object>()));
        };

        It should_set_the_status_of_the_request_to_no_route_available = () =>
        {
            requestFromDatabase.RequestStatusID.ShouldEqual((int)SAHL.X2Engine2.Enumerations.RequestStatus.NoRouteAvailable);
        };

        It should_update_the_request = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.Update<RequestDataModel>(Arg.Is<RequestDataModel>(request => request.RequestStoreComparer(requestFromDatabase))));
        };

        It should_increment_the_request_timeout_retries_value = () =>
            {
                requestFromDatabase.RequestTimeoutRetries.ShouldEqual(timeoutRetriesCount + 1);
            };
    }
}