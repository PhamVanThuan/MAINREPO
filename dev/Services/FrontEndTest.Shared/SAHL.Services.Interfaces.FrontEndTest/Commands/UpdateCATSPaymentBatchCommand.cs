using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class UpdateCATSPaymentBatchCommand : ServiceCommand, IFrontEndTestCommand
    {
        public UpdateCATSPaymentBatchCommand(CATSPaymentBatchDataModel catsPaymentBatch)
        {
            this.CATSPaymentBatch = catsPaymentBatch;
        }

        public CATSPaymentBatchDataModel CATSPaymentBatch { get; protected set; }
    }
}