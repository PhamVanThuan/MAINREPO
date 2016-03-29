using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.Application;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_getting_a_reserved_account_number : WithFakes
    {
        private static ApplicationDataManager applicationDataManager;
        private static FakeDbFactory dbFactory;
        private static int expectedAccountKey, reservedAccountKey;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);
            expectedAccountKey = 12345678;

            dbFactory.FakedDb.DbContext.WhenToldTo<IReadWriteSqlRepository>(x => x.Insert(Param.IsAny<AccountSequenceDataModel>()))
                .Callback<AccountSequenceDataModel>(y =>
                {
                    y.AccountKey = expectedAccountKey;
                });
        };

        private Because of = () =>
        {
            reservedAccountKey = applicationDataManager.GetReservedAccountNumber();
        };

        private It should_insert_a_new_account_sequence_record = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<AccountSequenceDataModel>(y => y.IsUsed.Value)));
        };

        private It should_return_the_inserted_account_number_ = () =>
        {
            reservedAccountKey.ShouldEqual(expectedAccountKey);
        };
    }
}
