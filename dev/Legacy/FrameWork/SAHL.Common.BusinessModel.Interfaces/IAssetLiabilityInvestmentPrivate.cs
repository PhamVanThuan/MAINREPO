using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    public partial interface IAssetLiabilityInvestmentPrivate : IEntityValidation, IBusinessModelObject, IAssetLiability
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityInvestmentPrivate_DAO.AssetValue
        /// </summary>
        System.Double AssetValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityInvestmentPrivate_DAO.CompanyName
        /// </summary>
        System.String CompanyName
        {
            get;
            set;
        }
    }
}