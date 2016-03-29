using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Groups application role types.
    /// </summary>
    public partial interface IApplicationRoleTypeGroup : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The description of the Application Role Type Group
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