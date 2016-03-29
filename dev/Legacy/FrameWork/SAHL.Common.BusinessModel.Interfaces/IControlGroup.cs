using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// The Control Group DAO Object specifies what type of control is being used.
    /// </summary>
    public partial interface IControlGroup : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}