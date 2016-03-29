using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// The LegalEntityRelationship_DAO class is used in order to store relationships that exist between Legal Entities involved
    /// in SA Home Loans accounts.
    /// </summary>
    public partial interface ILegalEntityRelationship : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the LegalEntity table. Each LegalEntityRelationship belongs to a single LegalEntity.
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the LegalEntity table. This is the Legal Entity which that referenced in the Legal Entity
        /// property is related to.
        /// </summary>
        ILegalEntity RelatedLegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to to the RelationshipType table. Each LegalEntityRelationship belongs to a single Relationship Type.
        /// </summary>
        ILegalEntityRelationshipType LegalEntityRelationshipType
        {
            get;
            set;
        }
    }
}