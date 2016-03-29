using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AssetLiability_DAO
    /// </summary>
    public partial interface IAssetLiability : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiability_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}