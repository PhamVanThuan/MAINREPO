using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// The LegalEntityAssetLiability_DAO class links the entries in the AssetLiability table to the LegalEntity
    /// </summary>
    public partial interface ILegalEntityAssetLiability : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The foreign key reference to the AssetLiability table where the information regarding the Asset/Liability is stored. Each
        /// LegalEntityAssetLiabilityKey belongs to a single AssetLiabilityKey.
        /// </summary>
        IAssetLiability AssetLiability
        {
            get;
            set;
        }

        /// <summary>
        /// The status of the record.
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Primary Key.
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the LegalEntity table where the information regarding the Legal Entity is stored. Each
        /// LegalEntityAssetLiabilityKey belongs to a single LegalEntityKey.
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }
    }
}