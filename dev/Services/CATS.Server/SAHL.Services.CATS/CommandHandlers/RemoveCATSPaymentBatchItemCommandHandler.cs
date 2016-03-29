using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Managers;
using SAHL.Services.Interfaces.CATS.Commands;

namespace SAHL.Services.CATS.CommandHandlers
{
    public class RemoveCATSPaymentBatchItemCommandHandler : IServiceCommandHandler<RemoveCATSPaymentBatchItemCommand>
    {
        private readonly ICATSDataManager _catsDataManager;

        public RemoveCATSPaymentBatchItemCommandHandler(ICATSDataManager catsDataManager)
        {
            _catsDataManager = catsDataManager;
        }

        public ISystemMessageCollection HandleCommand(RemoveCATSPaymentBatchItemCommand command,
            IServiceRequestMetadata metadata)
        {
            var messages = new SystemMessageCollection();
            _catsDataManager.RemoveCATSPaymentBatchItem(command.CATSPaymentBatchKey, command.GenericKey, command.GenericTypeKey);
            return messages;
        }
    }

}
