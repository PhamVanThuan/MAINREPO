using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountIndication_DAO
    /// </summary>
    public partial class AccountIndication : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AccountIndication_DAO>, IAccountIndication
    {
        public AccountIndication(SAHL.Common.BusinessModel.DAO.AccountIndication_DAO AccountIndication)
            : base(AccountIndication)
        {
            this._DAO = AccountIndication;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountIndication_DAO.AccountIndicator
        /// </summary>
        public Int32 AccountIndicator
        {
            get { return _DAO.AccountIndicator; }
            set { _DAO.AccountIndicator = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountIndication_DAO.Indicator
        /// </summary>
        public Char Indicator
        {
            get { return _DAO.Indicator; }
            set { _DAO.Indicator = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountIndication_DAO.UserID
        /// </summary>
        public String UserID
        {
            get { return _DAO.UserID; }
            set { _DAO.UserID = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountIndication_DAO.DateChange
        /// </summary>
        public DateTime DateChange
        {
            get { return _DAO.DateChange; }
            set { _DAO.DateChange = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountIndication_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountIndication_DAO.AccountIndicationType
        /// </summary>
        public IAccountIndicationType AccountIndicationType
        {
            get
            {
                if (null == _DAO.AccountIndicationType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAccountIndicationType, AccountIndicationType_DAO>(_DAO.AccountIndicationType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.AccountIndicationType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.AccountIndicationType = (AccountIndicationType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}