using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO
    /// </summary>
    public partial class AccountBaselII : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO>, IAccountBaselII
    {
        public AccountBaselII(SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO AccountBaselII)
            : base(AccountBaselII)
        {
            this._DAO = AccountBaselII;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.AccountingDate
        /// </summary>
        public DateTime AccountingDate
        {
            get { return _DAO.AccountingDate; }
            set { _DAO.AccountingDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.ProcessDate
        /// </summary>
        public DateTime ProcessDate
        {
            get { return _DAO.ProcessDate; }
            set { _DAO.ProcessDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.LGD
        /// </summary>
        public Double LGD
        {
            get { return _DAO.LGD; }
            set { _DAO.LGD = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.EAD
        /// </summary>
        public Double EAD
        {
            get { return _DAO.EAD; }
            set { _DAO.EAD = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.PD
        /// </summary>
        public Double PD
        {
            get { return _DAO.PD; }
            set { _DAO.PD = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.BehaviouralScore
        /// </summary>
        public Double BehaviouralScore
        {
            get { return _DAO.BehaviouralScore; }
            set { _DAO.BehaviouralScore = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.EL
        /// </summary>
        public Int32 EL
        {
            get { return _DAO.EL; }
            set { _DAO.EL = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.Account
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
    }
}