using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Balance_DAO
    /// </summary>
    public partial class Balance : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Balance_DAO>, IBalance
    {
        public Balance(SAHL.Common.BusinessModel.DAO.Balance_DAO Balance)
            : base(Balance)
        {
            this._DAO = Balance;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Balance_DAO.Amount
        /// </summary>
        public Double Amount
        {
            get { return _DAO.Amount; }
            set { _DAO.Amount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Balance_DAO.BalanceType
        /// </summary>
        public IBalanceType BalanceType
        {
            get
            {
                if (null == _DAO.BalanceType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IBalanceType, BalanceType_DAO>(_DAO.BalanceType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.BalanceType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.BalanceType = (BalanceType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Balance_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Balance_DAO.LoanBalance
        /// </summary>
        public ILoanBalance LoanBalance
        {
            get
            {
                if (null == _DAO.LoanBalance) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILoanBalance, LoanBalance_DAO>(_DAO.LoanBalance);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.LoanBalance = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.LoanBalance = (LoanBalance_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}