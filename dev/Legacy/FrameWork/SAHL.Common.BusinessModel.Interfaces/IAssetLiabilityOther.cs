using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    public partial interface IAssetLiabilityOther : IEntityValidation, IBusinessModelObject, IAssetLiability
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityOther_DAO.AssetValue
        /// </summary>
        System.Double AssetValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityOther_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityOther_DAO.LiabilityValue
        /// </summary>
        System.Double LiabilityValue
        {
            get;
            set;
        }
    }
}