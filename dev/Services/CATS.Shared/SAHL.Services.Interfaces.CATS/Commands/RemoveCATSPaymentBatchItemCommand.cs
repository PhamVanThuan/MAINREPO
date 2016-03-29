using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.CATS.Commands
{
    public class RemoveCATSPaymentBatchItemCommand : ServiceCommand, ICATSServiceCommand
    {
        public RemoveCATSPaymentBatchItemCommand(int catsPaymentBatchKey, int genericKey, int genericTypeKey)
        {
            CATSPaymentBatchKey = catsPaymentBatchKey;
            GenericKey = genericKey;
            GenericTypeKey = genericTypeKey;
        }

        public int GenericKey { get; protected set; }
        public int GenericTypeKey { get; protected set; }

        public int CATSPaymentBatchKey { get; protected set; }
    }
}