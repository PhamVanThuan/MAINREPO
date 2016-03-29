using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class InsertCATSPaymentBatchCommand : ServiceCommand, IFrontEndTestCommand
    {
        public InsertCATSPaymentBatchCommand(CATSPaymentBatchDataModel catsPaymentBatch, Guid catsPaymentBatchId)
        {
            this.CATSPaymentBatch = catsPaymentBatch;
            this.CATSPaymentBatchId = catsPaymentBatchId;
        }

        public CATSPaymentBatchDataModel CATSPaymentBatch { get; protected set; }

        public Guid CATSPaymentBatchId { get; protected set; }
    }
}