using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO
    /// </summary>
    public partial interface IFinancialServiceType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO.FinancialServiceGroup
        /// </summary>
        IFinancialServiceGroup FinancialServiceGroup
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO.OriginationSourceProductConfigurations
        /// </summary>
        IEventList<IOriginationSourceProductConfiguration> OriginationSourceProductConfigurations
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO.ProductConditions
        /// </summary>
        IEventList<IProductCondition> ProductConditions
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO.ResetConfiguration
        /// </summary>
        IResetConfiguration ResetConfiguration
        {
            get;
            set;
        }
    }
}