using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BankAccount_DAO
    /// </summary>
    public partial class BankAccount : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BankAccount_DAO>, IBankAccount
    {
        public BankAccount(SAHL.Common.BusinessModel.DAO.BankAccount_DAO BankAccount)
            : base(BankAccount)
        {
            this._DAO = BankAccount;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankAccount_DAO.ACBBranch
        /// </summary>
        public IACBBranch ACBBranch
        {
            get
            {
                if (null == _DAO.ACBBranch) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IACBBranch, ACBBranch_DAO>(_DAO.ACBBranch);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ACBBranch = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ACBBranch = (ACBBranch_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankAccount_DAO.AccountNumber
        /// </summary>
        public String AccountNumber
        {
            get { return _DAO.AccountNumber; }
            set { _DAO.AccountNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankAccount_DAO.ACBType
        /// </summary>
        public IACBType ACBType
        {
            get
            {
                if (null == _DAO.ACBType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IACBType, ACBType_DAO>(_DAO.ACBType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ACBType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ACBType = (ACBType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankAccount_DAO.AccountName
        /// </summary>
        public String AccountName
        {
            get { return _DAO.AccountName; }
            set { _DAO.AccountName = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankAccount_DAO.UserID
        /// </summary>
        public String UserID
        {
            get { return _DAO.UserID; }
            set { _DAO.UserID = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankAccount_DAO.ChangeDate
        /// </summary>
        public DateTime? ChangeDate
        {
            get { return _DAO.ChangeDate; }
            set { _DAO.ChangeDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankAccount_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}