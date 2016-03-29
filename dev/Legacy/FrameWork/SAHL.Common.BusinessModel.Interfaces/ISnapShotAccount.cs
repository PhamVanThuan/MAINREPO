using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO
    /// </summary>
    public partial interface ISnapShotAccount : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.Account
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.RemainingInstallments
        /// </summary>
        System.Int32 RemainingInstallments
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.Product
        /// </summary>
        IProduct Product
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.Valuation
        /// </summary>
        IValuation Valuation
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.InsertDate
        /// </summary>
        System.DateTime InsertDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.DebtCounselling
        /// </summary>
        IDebtCounselling DebtCounselling
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.HOCPremium
        /// </summary>
        System.Double HOCPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.LifePremium
        /// </summary>
        System.Double LifePremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.MonthlyServiceFee
        /// </summary>
        System.Double MonthlyServiceFee
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.SnapShotFinancialServices
        /// </summary>
        IEventList<ISnapShotFinancialService> SnapShotFinancialServices
        {
            get;
        }
    }
}