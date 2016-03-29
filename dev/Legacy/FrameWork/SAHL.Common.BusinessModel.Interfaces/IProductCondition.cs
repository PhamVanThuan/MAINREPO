using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ProductCondition_DAO
    /// </summary>
    public partial interface IProductCondition : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProductCondition_DAO.PurposeKey
        /// </summary>
        System.Int32 PurposeKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProductCondition_DAO.ApplicationName
        /// </summary>
        System.String ApplicationName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProductCondition_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProductCondition_DAO.Condition
        /// </summary>
        ICondition Condition
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProductCondition_DAO.FinancialServiceType
        /// </summary>
        IFinancialServiceType FinancialServiceType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProductCondition_DAO.OriginationSourceProduct
        /// </summary>
        IOriginationSourceProduct OriginationSourceProduct
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ProductCondition_DAO.ProductConditionType
        /// </summary>
        IProductConditionType ProductConditionType
        {
            get;
            set;
        }
    }
}