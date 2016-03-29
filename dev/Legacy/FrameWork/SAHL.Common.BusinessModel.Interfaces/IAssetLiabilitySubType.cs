using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AssetLiabilitySubType_DAO
    /// </summary>
    public partial interface IAssetLiabilitySubType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilitySubType_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilitySubType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilitySubType_DAO.AssetLiabilityType
        /// </summary>
        IAssetLiabilityType AssetLiabilityType
        {
            get;
            set;
        }
    }
}