using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Managers.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.CATS.Server.Specs.Managers.CatsDataManagerSpec
{
    public class when_removing_cats_payment_batch_item: WithFakes
    {
        private static ICATSDataManager dataManager;
        private static int catsPaymentBatchKey, genericKey, genericTypeKey;
        private static IDbFactory dbFactory;

        private Establish context = () =>
        {
            catsPaymentBatchKey = 3434;
            genericKey = 123;
            genericTypeKey = (int)GenericKeyType.ThirdParty;
            dbFactory = new FakeDbFactory();
            dataManager = new CATSDataManager(dbFactory);
        };

        private Because of = () =>
        {
            dataManager.RemoveCATSPaymentBatchItem(catsPaymentBatchKey, genericKey, genericTypeKey);
        };

        private It should_update_the_cats_payment_batch_status_to_failed = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(x => x.ExecuteNonQuery(Arg.Is<RemoveCATSPaymentBatchItemStatement>(
                y => y.CATSPaymentBatchKey == catsPaymentBatchKey && y.GenericKey == genericKey && y.GenericTypeKey == genericTypeKey)));
        };

        private It should_complete = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(x => x.Complete());
        };
    }
}
