using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// The ApplicationStatus_DAO class specifies the different statuses that an Application can have.
    /// </summary>
    public partial interface IApplicationStatus : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The description of the Application Status
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