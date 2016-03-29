using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.CATS.CommandHandlers;
using SAHL.Services.CATS.Managers;
using SAHL.Services.Interfaces.CATS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;

namespace SAHL.Services.CATS.Server.Specs.HandlerSpec.MarkCATSPaymentBatchAsFailed
{
    public class when_marking_cats_payment_batch_as_failed : WithCoreFakes
    {
        private static ICATSDataManager cATSDataManager;
        private static MarkCATSPaymentBatchAsFailedCommand command;
        private static MarkCATSPaymentBatchAsFailedCommandHandler handler;
        private static int catsPaymentBatchKey;
        private static IServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            cATSDataManager = An<ICATSDataManager>();
            catsPaymentBatchKey = 1234;
            metadata=An<IServiceRequestMetadata>();
            command = new MarkCATSPaymentBatchAsFailedCommand(catsPaymentBatchKey);
            handler = new MarkCATSPaymentBatchAsFailedCommandHandler(cATSDataManager);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, metadata);
        };

        private It should_set_paytment_batch_as_failed = () =>
        {
            cATSDataManager.WasToldTo(x => x.SetCATSPaymentBatchAsFailed(catsPaymentBatchKey));
        };
    }
}
