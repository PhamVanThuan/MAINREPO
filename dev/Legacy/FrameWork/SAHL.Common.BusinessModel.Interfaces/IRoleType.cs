using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// RoleType_DAO contains the different types of Roles that a Legal Entity can play on an Account at SAHL. This would include:
    /// Main ApplicantSuretorPrevious InsurerAssured Life
    /// </summary>
    public partial interface IRoleType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The description of the Role Type
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