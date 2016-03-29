using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class RemoveCATSPaymentBatchCommand : ServiceCommand, IFrontEndTestCommand
    {
        public RemoveCATSPaymentBatchCommand(int catsPaymentBatchKey)
        {
            this.CATSPaymentBatchKey = catsPaymentBatchKey;
        }

        public int CATSPaymentBatchKey { get; protected set; }
    }
}