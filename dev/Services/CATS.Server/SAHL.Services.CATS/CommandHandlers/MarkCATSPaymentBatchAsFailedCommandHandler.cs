using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Managers;
using SAHL.Services.Interfaces.CATS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.CATS.CommandHandlers
{
    public class MarkCATSPaymentBatchAsFailedCommandHandler : IServiceCommandHandler<MarkCATSPaymentBatchAsFailedCommand>
    {
        private ICATSDataManager catsDataManager;

        public MarkCATSPaymentBatchAsFailedCommandHandler(ICATSDataManager catsDataManager)
        {
            this.catsDataManager = catsDataManager;
        }

        public ISystemMessageCollection HandleCommand(MarkCATSPaymentBatchAsFailedCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            catsDataManager.SetCATSPaymentBatchAsFailed(command.CATSPaymentBatchKey);

            return messages;
        }
    }
}
