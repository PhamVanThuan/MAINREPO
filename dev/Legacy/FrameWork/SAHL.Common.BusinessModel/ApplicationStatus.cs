using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// The ApplicationStatus_DAO class specifies the different statuses that an Application can have.
    /// </summary>
    public partial class ApplicationStatus : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationStatus_DAO>, IApplicationStatus
    {
        public ApplicationStatus(SAHL.Common.BusinessModel.DAO.ApplicationStatus_DAO ApplicationStatus)
            : base(ApplicationStatus)
        {
            this._DAO = ApplicationStatus;
        }

        /// <summary>
        /// The description of the Application Status
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}