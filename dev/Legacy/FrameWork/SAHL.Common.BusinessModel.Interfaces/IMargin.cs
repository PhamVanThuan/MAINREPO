using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Margin_DAO
    /// </summary>
    public partial interface IMargin : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Margin_DAO.Value
        /// </summary>
        System.Double Value
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Margin_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Margin_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Margin_DAO.MarginProducts
        /// </summary>
        IEventList<IMarginProduct> MarginProducts
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Margin_DAO.ProductCategories
        /// </summary>
        IEventList<IProductCategory> ProductCategories
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Margin_DAO.RateConfigurations
        /// </summary>
        IEventList<IRateConfiguration> RateConfigurations
        {
            get;
        }
    }
}