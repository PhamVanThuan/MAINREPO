using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO
    /// </summary>
    public partial interface IAccountBaselII : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.AccountingDate
        /// </summary>
        System.DateTime AccountingDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.ProcessDate
        /// </summary>
        System.DateTime ProcessDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.LGD
        /// </summary>
        System.Double LGD
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.EAD
        /// </summary>
        System.Double EAD
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.PD
        /// </summary>
        System.Double PD
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.BehaviouralScore
        /// </summary>
        System.Double BehaviouralScore
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.EL
        /// </summary>
        System.Int32 EL
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountBaselII_DAO.Account
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }
    }
}