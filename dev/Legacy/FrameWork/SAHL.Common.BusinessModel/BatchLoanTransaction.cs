using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BatchLoanTransaction_DAO
    /// </summary>
    public partial class BatchLoanTransaction : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BatchLoanTransaction_DAO>, IBatchLoanTransaction
    {
        public BatchLoanTransaction(SAHL.Common.BusinessModel.DAO.BatchLoanTransaction_DAO BatchLoanTransaction)
            : base(BatchLoanTransaction)
        {
            this._DAO = BatchLoanTransaction;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchLoanTransaction_DAO.LoanTransactionNumber
        /// </summary>
        public Int32 LoanTransactionNumber
        {
            get { return _DAO.LoanTransactionNumber; }
            set { _DAO.LoanTransactionNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchLoanTransaction_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchLoanTransaction_DAO.BatchTransaction
        /// </summary>
        public IBatchTransaction BatchTransaction
        {
            get
            {
                if (null == _DAO.BatchTransaction) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IBatchTransaction, BatchTransaction_DAO>(_DAO.BatchTransaction);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.BatchTransaction = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.BatchTransaction = (BatchTransaction_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}