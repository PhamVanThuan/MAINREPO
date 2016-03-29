using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.CATS.Managers.Statements;
using SAHL.Services.CATS.Utils;
using SAHL.Services.Interfaces.CATS.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.CATS.Managers
{
    public class CATSDataManager : ICATSDataManager
    {
        private IDbFactory dbFactory;

        public CATSDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public int GetNewThirdPartyPaymentBatchReference(CATSPaymentBatchType catsPaymentBatchType)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var query = new GetNewThirdPartyPaymentReferenceStatement(catsPaymentBatchType);
                var batchKey = db.SelectOne(query);
                db.Complete();
                return batchKey;
            }
        }

        public void InsertThirdPartyInvoicePaymentBatchItem(CATSPaymentBatchItemModel catsPaymentBatchItemModel)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var sqlStatement = new InsertCATSPaymentBatchItemStatement(catsPaymentBatchItemModel);
                db.Insert<CATSPaymentBatchItemDataModel>(sqlStatement);
                db.Complete();
            }
        }

        public IEnumerable<CATSPaymentBatchItemDataModel> GetPaymentBatchLineItemsByBatchKey(int batchKey)
        {
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var query = new GetPaymentBatchLineItemsByBatchKeyStatement(batchKey);
                return db.Select(query);
            }
        }

        public BankAccountDataModel GetBankingAccountByKey(int bankAccountKey)
        {
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var query = new GetBankAccountDataModelByKeyStatement(bankAccountKey);
                return db.SelectOne(query);
            }
        }

        public CATSPaymentBatchTypeDataModel GetBatchTypeInfo(int paymentBatchNumber)
        {
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var query = new GetPaymentBatchTypeByPaymentBatchKeyStatement(paymentBatchNumber);
                return db.SelectOne(query);
            }
        }

        public CATSPaymentBatchDataModel GetBatchByKey(int batchNumber)
        {
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var query = new GetBatchByKeyStatement(batchNumber);
                return db.SelectOne(query);
            }
        }

        public void CloseCatsPaymentBatch(int paymentBatchNumber, int paymentBatchStatus, int fileSequenceNumber, string filename)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var query = new UpdatePaymentBatchStatusStatement(paymentBatchNumber, paymentBatchStatus, fileSequenceNumber, filename);
                db.Update<CATSPaymentBatchStatusDataModel>(query);
                db.Complete();
            }
        }

        public void RemoveCATSPaymentBatchItem(int catsPaymentBatchKey, int genericKey, int genericTypeKey)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var query = new RemoveCATSPaymentBatchItemStatement(catsPaymentBatchKey, genericKey, genericTypeKey);
                db.ExecuteNonQuery(query);
                db.Complete();
            }
        }

        public CATSPaymentBatchDataModel GetLastProcessedBatch(CATSPaymentBatchType batchType)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var query = new GetLastProcessedBatchQueryStatement(CATSPaymentBatchType.ThirdPartyInvoice);
                return db.SelectOne(query);
            }
        }

        public int GetCATSPaymentBatchSequenceNumber()
        {
            int catsControlNumber = (int)CatsControlType.DisbursementInProcess;
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var query = new GetControlTableValueByControlNumberStatement(catsControlNumber);
                var sequenceNumber = db.SelectOne(query);

                sequenceNumber = sequenceNumber + 1;

                db.Complete();
                return Convert.ToInt32(sequenceNumber);
            }
        }

        public void SetCatsPaymentBatchSequenceNumber(int catsPaymentSequenceNumber)
        {
            int catsControlNumber = (int)CatsControlType.DisbursementInProcess;
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var query = new UpdateControlTableNumericValueStatement(catsPaymentSequenceNumber, catsControlNumber);
                db.ExecuteNonQuery(query);
                db.Complete();
            }
        }

        public CATSPaymentBatchTypeDataModel GetBatchTypeByKey(int batchTypeKey)
        {
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOneWhere<CATSPaymentBatchTypeDataModel>(string.Format("CATSPaymentBatchTypeKey = {0}", batchTypeKey));
            }
        }

        public void SetCATSPaymentBatchAsFailed(int catsPaymentBatchKey)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var query = new SetCATSPaymentBatchAsFailedStatement(catsPaymentBatchKey);
                db.ExecuteNonQuery(query);
                db.Complete();
            }
        }
    }
}
