using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// The ApplicationType_DAO class specifies the different types of Applications.
    /// </summary>
    public partial interface IApplicationType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The Description of the Application Type e.g. Readvance/Further Loan
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