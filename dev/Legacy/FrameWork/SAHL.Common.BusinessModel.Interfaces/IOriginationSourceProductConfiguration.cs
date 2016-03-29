using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.OriginationSourceProductConfiguration_DAO
    /// </summary>
    public partial interface IOriginationSourceProductConfiguration : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OriginationSourceProductConfiguration_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OriginationSourceProductConfiguration_DAO.FinancialServiceType
        /// </summary>
        IFinancialServiceType FinancialServiceType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OriginationSourceProductConfiguration_DAO.MarketRate
        /// </summary>
        IMarketRate MarketRate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OriginationSourceProductConfiguration_DAO.OriginationSourceProduct
        /// </summary>
        IOriginationSourceProduct OriginationSourceProduct
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OriginationSourceProductConfiguration_DAO.ResetConfiguration
        /// </summary>
        IResetConfiguration ResetConfiguration
        {
            get;
            set;
        }
    }
}