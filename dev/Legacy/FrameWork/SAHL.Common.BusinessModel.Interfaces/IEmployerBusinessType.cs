using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// EmployerBusinessType_DAO is used to hold the different business types which can be applied to an Employer.
    /// </summary>
    public partial interface IEmployerBusinessType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The description of the Employer Business type. e.g. Company/Sole Proprietor
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