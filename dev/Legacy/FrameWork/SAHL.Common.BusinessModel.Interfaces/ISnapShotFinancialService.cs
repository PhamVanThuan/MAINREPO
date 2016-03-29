using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO
    /// </summary>
    public partial interface ISnapShotFinancialService : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.Account
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.FinancialService
        /// </summary>
        IFinancialService FinancialService
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.FinancialServiceType
        /// </summary>
        IFinancialServiceType FinancialServiceType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.ResetConfiguration
        /// </summary>
        IResetConfiguration ResetConfiguration
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.ActiveMarketRate
        /// </summary>
        System.Double ActiveMarketRate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.Margin
        /// </summary>
        IMargin Margin
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.Installment
        /// </summary>
        System.Double Installment
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.SnapShotFinancialAdjustments
        /// </summary>
        IEventList<ISnapShotFinancialAdjustment> SnapShotFinancialAdjustments
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.SnapShotAccount
        /// </summary>
        ISnapShotAccount SnapShotAccount
        {
            get;
            set;
        }
    }
}