using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.CATS.Commands
{
    public class SummarisePaymentsToRecipientCommand : ServiceCommand, ICATSServiceCommand, IRequiresClient
    {
        public int ClientKey { get; protected set; }

        public IEnumerable<CATSPaymentBatchItemDataModel> PaymentsCollection { get; protected set; }

        public SummarisePaymentsToRecipientCommand(int clientKey, IEnumerable<CATSPaymentBatchItemDataModel> paymentsCollection)
        {
            ClientKey = clientKey;
            PaymentsCollection = paymentsCollection;
        }
    }
}
