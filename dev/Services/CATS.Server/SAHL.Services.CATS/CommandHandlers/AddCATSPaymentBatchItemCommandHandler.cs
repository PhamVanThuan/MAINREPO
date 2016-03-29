using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Managers;
using SAHL.Services.Interfaces.CATS.Commands;

namespace SAHL.Services.CATS.CommandHandlers
{
    public class AddCATSPaymentBatchItemCommandHandler : IServiceCommandHandler<AddCATSPaymentBatchItemCommand>
    {
        private readonly ICATSDataManager _catsDataManager;

        public AddCATSPaymentBatchItemCommandHandler(ICATSDataManager catsDataManager)
        {
            _catsDataManager = catsDataManager;
        }

        public ISystemMessageCollection HandleCommand(AddCATSPaymentBatchItemCommand command,
            IServiceRequestMetadata metadata)
        {
            var messages = new SystemMessageCollection();
            _catsDataManager.InsertThirdPartyInvoicePaymentBatchItem(command.CATSPaymentBatchItemModel);
            return messages;
        }
    }
}
