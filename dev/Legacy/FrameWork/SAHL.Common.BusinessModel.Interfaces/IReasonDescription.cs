using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ReasonDescription_DAO contains the text versions of the Reasons. It also links the Reason to a Translatable Item
    /// from which a translated version of the reason can be taken.
    /// </summary>
    public partial interface IReasonDescription : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The text description of the Reason
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

        /// <summary>
        /// Each Reason belongs to a Translatable Item in the TranslatableItem table. This is the foreign key reference to the
        /// TranslatableItem table.
        /// </summary>
        ITranslatableItem TranslatableItem
        {
            get;
            set;
        }
    }
}