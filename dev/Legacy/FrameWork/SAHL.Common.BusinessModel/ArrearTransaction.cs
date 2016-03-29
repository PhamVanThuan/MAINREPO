using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO
    /// </summary>
    public partial class ArrearTransaction : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO>, IArrearTransaction
    {
        public ArrearTransaction(SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO ArrearTransaction)
            : base(ArrearTransaction)
        {
            this._DAO = ArrearTransaction;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.InsertDate
        /// </summary>
        public DateTime InsertDate
        {
            get { return _DAO.InsertDate; }
            set { _DAO.InsertDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.EffectiveDate
        /// </summary>
        public DateTime EffectiveDate
        {
            get { return _DAO.EffectiveDate; }
            set { _DAO.EffectiveDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.CorrectionDate
        /// </summary>
        public DateTime? CorrectionDate
        {
            get { return _DAO.CorrectionDate; }
            set { _DAO.CorrectionDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.Amount
        /// </summary>
        public Double Amount
        {
            get { return _DAO.Amount; }
            set { _DAO.Amount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.Balance
        /// </summary>
        public Double Balance
        {
            get { return _DAO.Balance; }
            set { _DAO.Balance = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.Reference
        /// </summary>
        public String Reference
        {
            get { return _DAO.Reference; }
            set { _DAO.Reference = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.Userid
        /// </summary>
        public String Userid
        {
            get { return _DAO.Userid; }
            set { _DAO.Userid = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.IsRolledBack
        /// </summary>
        public Boolean IsRolledBack
        {
            get { return _DAO.IsRolledBack; }
            set { _DAO.IsRolledBack = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.FinancialService
        /// </summary>
        public IFinancialService FinancialService
        {
            get
            {
                if (null == _DAO.FinancialService) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IFinancialService, FinancialService_DAO>(_DAO.FinancialService);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.FinancialService = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.FinancialService = (FinancialService_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.TransactionType
        /// </summary>
        public ITransactionType TransactionType
        {
            get
            {
                if (null == _DAO.TransactionType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ITransactionType, TransactionType_DAO>(_DAO.TransactionType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.TransactionType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.TransactionType = (TransactionType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}