using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedLongTermInvestment_DAO
    /// </summary>
    public partial interface IAssetLiabilityFixedLongTermInvestment : IEntityValidation, IBusinessModelObject, IAssetLiability
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedLongTermInvestment_DAO.LiabilityValue
        /// </summary>
        System.Double LiabilityValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedLongTermInvestment_DAO.CompanyName
        /// </summary>
        System.String CompanyName
        {
            get;
            set;
        }
    }
}