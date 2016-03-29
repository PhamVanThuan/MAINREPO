using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2.Messages;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Services;


namespace SAHL.X2Engine2.Specs.X2RequestStoreSpecs
{
    internal class when_asking_for_number_of_retries : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestStore> autoMocker;

        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static X2CreateInstanceRequest createInstanceRequest;
        private static RequestDataModel requestFromDatabase;
        private static DateTime dateTime;
        private static int numberOfRetries;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestStore>();

            dateTime = DateTime.Now;
            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            createInstanceRequest = new X2CreateInstanceRequest(Guid.NewGuid(), "activityName", "processName", "workflowName", serviceRequestMetadata, false);
            requestFromDatabase = new RequestDataModel(createInstanceRequest.CorrelationId, Guid.NewGuid().ToString(), 1, dateTime, dateTime, 1);
            readWriteSqlRepository.WhenToldTo(x => x.SelectOne<RequestDataModel>(Param.IsAny<string>(), Param.IsAny<object>())).Return(requestFromDatabase);
        };

        private Because of = () =>
        {
            numberOfRetries = autoMocker.ClassUnderTest.GetNumberOfTimeouts(createInstanceRequest);
        };

        private It should_get_the_number_of_retries = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.SelectOne<RequestDataModel>(Param.IsAny<string>(), Param.IsAny<object>()));
        };

        private It should_get_the_correct_number_of_retries = () =>
        {
            numberOfRetries.ShouldEqual(1);
        };
    }
}