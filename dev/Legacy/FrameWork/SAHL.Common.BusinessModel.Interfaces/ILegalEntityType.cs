using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// LegalEntityType_DAO is used in order to store the different Legal Entity types that exist. The LegalEntityType forms
    /// the basis for the Legal Entity discrimination.
    /// </summary>
    public partial interface ILegalEntityType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The Legal Entity Type Description. e.g. Natural Person/Trust
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