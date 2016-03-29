using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.CATS.Managers.Statements
{
    public class UpdatePaymentBatchStatusStatement : ISqlStatement<CATSPaymentBatchStatusDataModel>
    {
        public int PaymentBatchStatus { get; protected set; }
        public int PaymentBatchKey { get; protected set; }
        public int FileSequenceNumber { get; protected set; }
        public string Filename { get; protected set; }

        public UpdatePaymentBatchStatusStatement(int paymentBatchKey, int paymentBatchStatus, int fileSequenceNumber, string filename)
        {
            this.PaymentBatchKey = paymentBatchKey;
            this.PaymentBatchStatus = paymentBatchStatus;
            this.FileSequenceNumber = fileSequenceNumber;
            this.Filename = filename;
        }

        public string GetStatement()
        {
            return @"UPDATE [2AM].[dbo].[CATSPaymentBatch]
                       SET [CATSPaymentBatchStatusKey] = @PaymentBatchStatus
                          ,[ProcessedDate] = GETDATE()
                          ,[CATSFileSequenceNo] = @FileSequenceNumber
                          ,[CATSFileName]= @Filename
                     WHERE [CATSPaymentBatchKey] = @PaymentBatchKey";
        }
    }
}
