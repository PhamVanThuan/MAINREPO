using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountInformation_DAO
    /// </summary>
    public partial class AccountInformation : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AccountInformation_DAO>, IAccountInformation
    {
        public AccountInformation(SAHL.Common.BusinessModel.DAO.AccountInformation_DAO AccountInformation)
            : base(AccountInformation)
        {
            this._DAO = AccountInformation;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountInformation_DAO.EntryDate
        /// </summary>
        public DateTime? EntryDate
        {
            get { return _DAO.EntryDate; }
            set { _DAO.EntryDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountInformation_DAO.Amount
        /// </summary>
        public Double? Amount
        {
            get { return _DAO.Amount; }
            set { _DAO.Amount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountInformation_DAO.Information
        /// </summary>
        public String Information
        {
            get { return _DAO.Information; }
            set { _DAO.Information = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountInformation_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountInformation_DAO.Account
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
        /// SAHL.Common.BusinessModel.DAO.AccountInformation_DAO.AccountInformationType
        /// </summary>
        public IAccountInformationType AccountInformationType
        {
            get
            {
                if (null == _DAO.AccountInformationType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAccountInformationType, AccountInformationType_DAO>(_DAO.AccountInformationType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.AccountInformationType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.AccountInformationType = (AccountInformationType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}