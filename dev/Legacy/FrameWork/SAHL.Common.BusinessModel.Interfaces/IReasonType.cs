using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ReasonType_DAO is used to assign each reason a type. It is also specifies the Generic Key type which applies to the Reason.
    /// </summary>
    public partial interface IReasonType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The Description of the Reason Type.
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// Each Reason Type (and the Reasons associated to the Reason Type) belong to a specific GenericKeyType. For example
        /// a Reason Type of 'Credit Decline Loan' would be linked to an OfferInformationKey via this GenericKeyType link.
        /// </summary>
        IGenericKeyType GenericKeyType
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

        /// <summary>
        /// Each of the ReasonTypes belong to a ReasonTypeGroup. This is the foreign key reference to the ReasonTypeGroup table.
        /// </summary>
        IReasonTypeGroup ReasonTypeGroup
        {
            get;
            set;
        }
    }
}