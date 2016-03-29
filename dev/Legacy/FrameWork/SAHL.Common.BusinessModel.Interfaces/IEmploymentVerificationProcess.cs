using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.EmploymentVerificationProcess_DAO
    /// </summary>
    public partial interface IEmploymentVerificationProcess : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.EmploymentVerificationProcess_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.EmploymentVerificationProcess_DAO.Employment
        /// </summary>
        IEmployment Employment
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.EmploymentVerificationProcess_DAO.EmploymentVerificationProcessType
        /// </summary>
        IEmploymentVerificationProcessType EmploymentVerificationProcessType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.EmploymentVerificationProcess_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.EmploymentVerificationProcess_DAO.ChangeDate
        /// </summary>
        DateTime? ChangeDate
        {
            get;
            set;
        }
    }
}