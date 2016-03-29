using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.CATS.Models;
using System.Collections.Generic;

namespace SAHL.Services.CATS.Managers
{
    public interface ICATSDataManager
    {
        int GetNewThirdPartyPaymentBatchReference(CATSPaymentBatchType batchType);

        void InsertThirdPartyInvoicePaymentBatchItem(CATSPaymentBatchItemModel thirdPartyInvoicePaymentBatchItemModel);

        IEnumerable<CATSPaymentBatchItemDataModel> GetPaymentBatchLineItemsByBatchKey(int batchNumber);

        BankAccountDataModel GetBankingAccountByKey(int soucreAccount);

        CATSPaymentBatchTypeDataModel GetBatchTypeInfo(int batchNumber);

        CATSPaymentBatchDataModel GetBatchByKey(int batchNumber);

        void CloseCatsPaymentBatch(int batchNumber, int catsPaymentBatchStatus, int fileSequenceNumber, string filename);

        CATSPaymentBatchDataModel GetLastProcessedBatch(CATSPaymentBatchType batchType);

        void SetCatsPaymentBatchSequenceNumber(int catsPaymentSequenceNumber);

        void RemoveCATSPaymentBatchItem(int catsPaymentBatchKey, int genericKey, int genericTypeKey);

        int GetCATSPaymentBatchSequenceNumber();

        CATSPaymentBatchTypeDataModel GetBatchTypeByKey(int batchTypeKey);

        void SetCATSPaymentBatchAsFailed(int catsPaymentBatchKey);
    }
}