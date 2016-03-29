using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO
    /// </summary>
    public partial class CommissionTransaction : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO>, ICommissionTransaction
    {
        public CommissionTransaction(SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO CommissionTransaction)
            : base(CommissionTransaction)
        {
            this._DAO = CommissionTransaction;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.FinancialServiceKey
        /// </summary>
        public Int32 FinancialServiceKey
        {
            get { return _DAO.FinancialServiceKey; }
            set { _DAO.FinancialServiceKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.CommissionCalcAmount
        /// </summary>
        public Double CommissionCalcAmount
        {
            get { return _DAO.CommissionCalcAmount; }
            set { _DAO.CommissionCalcAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.CommissionAmount
        /// </summary>
        public Double? CommissionAmount
        {
            get { return _DAO.CommissionAmount; }
            set { _DAO.CommissionAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.CommissionFactor
        /// </summary>
        public Decimal? CommissionFactor
        {
            get { return _DAO.CommissionFactor; }
            set { _DAO.CommissionFactor = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.CommissionType
        /// </summary>
        public String CommissionType
        {
            get { return _DAO.CommissionType; }
            set { _DAO.CommissionType = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.KickerCalcAmount
        /// </summary>
        public Double? KickerCalcAmount
        {
            get { return _DAO.KickerCalcAmount; }
            set { _DAO.KickerCalcAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.KickerAmount
        /// </summary>
        public Double? KickerAmount
        {
            get { return _DAO.KickerAmount; }
            set { _DAO.KickerAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.TransactionDate
        /// </summary>
        public DateTime TransactionDate
        {
            get { return _DAO.TransactionDate; }
            set { _DAO.TransactionDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.BatchRunDate
        /// </summary>
        public DateTime? BatchRunDate
        {
            get { return _DAO.BatchRunDate; }
            set { _DAO.BatchRunDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.ADUser
        /// </summary>
        public IADUser ADUser
        {
            get
            {
                if (null == _DAO.ADUser) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IADUser, ADUser_DAO>(_DAO.ADUser);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ADUser = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ADUser = (ADUser_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CommissionTransaction_DAO.FinancialServiceType
        /// </summary>
        public IFinancialServiceType FinancialServiceType
        {
            get
            {
                if (null == _DAO.FinancialServiceType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IFinancialServiceType, FinancialServiceType_DAO>(_DAO.FinancialServiceType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.FinancialServiceType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.FinancialServiceType = (FinancialServiceType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}