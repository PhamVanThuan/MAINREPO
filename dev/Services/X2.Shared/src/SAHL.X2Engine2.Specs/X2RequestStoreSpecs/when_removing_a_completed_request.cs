using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.X2RequestStoreSpecs
{
    public class when_removing_a_completed_request : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestStore> autoMocker;

        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static Guid requestID;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestStore>();

            requestID = Guid.NewGuid();
            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.RemoveCompletedRequest(requestID);
        };

        private It should_remove_the_request = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.DeleteByKey<RequestDataModel, Guid>(requestID));
        };
    }
}