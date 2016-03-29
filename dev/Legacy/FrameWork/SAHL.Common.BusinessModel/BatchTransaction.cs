using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BatchTransaction_DAO
    /// </summary>
    public partial class BatchTransaction : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BatchTransaction_DAO>, IBatchTransaction
    {
        public BatchTransaction(SAHL.Common.BusinessModel.DAO.BatchTransaction_DAO BatchTransaction)
            : base(BatchTransaction)
        {
            this._DAO = BatchTransaction;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTransaction_DAO.BatchTransactionStatus
        /// </summary>
        public IBatchTransactionStatus BatchTransactionStatus
        {
            get
            {
                if (null == _DAO.BatchTransactionStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IBatchTransactionStatus, BatchTransactionStatus_DAO>(_DAO.BatchTransactionStatus);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.BatchTransactionStatus = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.BatchTransactionStatus = (BatchTransactionStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTransaction_DAO.TransactionTypeNumber
        /// </summary>
        public Int32 TransactionTypeNumber
        {
            get { return _DAO.TransactionTypeNumber; }
            set { _DAO.TransactionTypeNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTransaction_DAO.EffectiveDate
        /// </summary>
        public DateTime EffectiveDate
        {
            get { return _DAO.EffectiveDate; }
            set { _DAO.EffectiveDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTransaction_DAO.Amount
        /// </summary>
        public Double Amount
        {
            get { return _DAO.Amount; }
            set { _DAO.Amount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTransaction_DAO.Reference
        /// </summary>
        public String Reference
        {
            get { return _DAO.Reference; }
            set { _DAO.Reference = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTransaction_DAO.UserID
        /// </summary>
        public String UserID
        {
            get { return _DAO.UserID; }
            set { _DAO.UserID = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTransaction_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTransaction_DAO.BatchLoanTransactions
        /// </summary>
        private DAOEventList<BatchLoanTransaction_DAO, IBatchLoanTransaction, BatchLoanTransaction> _BatchLoanTransactions;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTransaction_DAO.BatchLoanTransactions
        /// </summary>
        public IEventList<IBatchLoanTransaction> BatchLoanTransactions
        {
            get
            {
                if (null == _BatchLoanTransactions)
                {
                    if (null == _DAO.BatchLoanTransactions)
                        _DAO.BatchLoanTransactions = new List<BatchLoanTransaction_DAO>();
                    _BatchLoanTransactions = new DAOEventList<BatchLoanTransaction_DAO, IBatchLoanTransaction, BatchLoanTransaction>(_DAO.BatchLoanTransactions);
                    _BatchLoanTransactions.BeforeAdd += new EventListHandler(OnBatchLoanTransactions_BeforeAdd);
                    _BatchLoanTransactions.BeforeRemove += new EventListHandler(OnBatchLoanTransactions_BeforeRemove);
                    _BatchLoanTransactions.AfterAdd += new EventListHandler(OnBatchLoanTransactions_AfterAdd);
                    _BatchLoanTransactions.AfterRemove += new EventListHandler(OnBatchLoanTransactions_AfterRemove);
                }
                return _BatchLoanTransactions;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTransaction_DAO.Account
        /// </summary>
        public IAccount Account
        {
            get
            {
                if (null == _DAO.Account) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAccount, Account_DAO>(_DAO.Account);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Account = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Account = (Account_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTransaction_DAO.BulkBatch
        /// </summary>
        public IBulkBatch BulkBatch
        {
            get
            {
                if (null == _DAO.BulkBatch) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IBulkBatch, BulkBatch_DAO>(_DAO.BulkBatch);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.BulkBatch = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.BulkBatch = (BulkBatch_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTransaction_DAO.LegalEntity
        /// </summary>
        public ILegalEntity LegalEntity
        {
            get
            {
                if (null == _DAO.LegalEntity) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.LegalEntity);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.LegalEntity = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.LegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _BatchLoanTransactions = null;
        }
    }
}