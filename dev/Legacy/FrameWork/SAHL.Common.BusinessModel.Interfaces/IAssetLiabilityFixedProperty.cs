using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    public partial interface IAssetLiabilityFixedProperty : IEntityValidation, IBusinessModelObject, IAssetLiability
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedProperty_DAO.DateAcquired
        /// </summary>
        DateTime? DateAcquired
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedProperty_DAO.Address
        /// </summary>
        IAddress Address
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedProperty_DAO.AssetValue
        /// </summary>
        System.Double AssetValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedProperty_DAO.LiabilityValue
        /// </summary>
        System.Double LiabilityValue
        {
            get;
            set;
        }
    }
}