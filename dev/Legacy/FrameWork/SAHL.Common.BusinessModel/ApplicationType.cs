using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// The ApplicationType_DAO class specifies the different types of Applications.
    /// </summary>
    public partial class ApplicationType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationType_DAO>, IApplicationType
    {
        public ApplicationType(SAHL.Common.BusinessModel.DAO.ApplicationType_DAO ApplicationType)
            : base(ApplicationType)
        {
            this._DAO = ApplicationType;
        }

        /// <summary>
        /// The Description of the Application Type e.g. Readvance/Further Loan
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