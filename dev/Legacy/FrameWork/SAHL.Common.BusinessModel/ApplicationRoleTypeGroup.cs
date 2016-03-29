using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Groups application role types.
    /// </summary>
    public partial class ApplicationRoleTypeGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationRoleTypeGroup_DAO>, IApplicationRoleTypeGroup
    {
        public ApplicationRoleTypeGroup(SAHL.Common.BusinessModel.DAO.ApplicationRoleTypeGroup_DAO ApplicationRoleTypeGroup)
            : base(ApplicationRoleTypeGroup)
        {
            this._DAO = ApplicationRoleTypeGroup;
        }

        /// <summary>
        /// The description of the Application Role Type Group
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