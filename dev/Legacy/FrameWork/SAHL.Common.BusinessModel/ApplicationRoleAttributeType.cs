using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationRoleAttributeType_DAO
    /// </summary>
    public partial class ApplicationRoleAttributeType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationRoleAttributeType_DAO>, IApplicationRoleAttributeType
    {
        public ApplicationRoleAttributeType(SAHL.Common.BusinessModel.DAO.ApplicationRoleAttributeType_DAO ApplicationRoleAttributeType)
            : base(ApplicationRoleAttributeType)
        {
            this._DAO = ApplicationRoleAttributeType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationRoleAttributeType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationRoleAttributeType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}