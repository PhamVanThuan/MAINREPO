using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// LegalEntityRelationshipType_DAO is used to store the different relationship types that can exist between Legal Entities.
    /// </summary>
    public partial interface ILegalEntityRelationshipType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The description of the relationship. e.g. Spouse/Partner
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}