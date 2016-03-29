using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    public partial interface IAssetLiabilityLifeAssurance : IEntityValidation, IBusinessModelObject, IAssetLiability
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityLifeAssurance_DAO.CompanyName
        /// </summary>
        System.String CompanyName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityLifeAssurance_DAO.SurrenderValue
        /// </summary>
        System.Double SurrenderValue
        {
            get;
            set;
        }
    }
}