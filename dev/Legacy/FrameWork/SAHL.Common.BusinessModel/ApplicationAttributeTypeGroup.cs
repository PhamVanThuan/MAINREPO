using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeTypeGroup_DAO
    /// </summary>
    public partial class ApplicationAttributeTypeGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationAttributeTypeGroup_DAO>, IApplicationAttributeTypeGroup
    {
        public ApplicationAttributeTypeGroup(SAHL.Common.BusinessModel.DAO.ApplicationAttributeTypeGroup_DAO ApplicationAttributeTypeGroup)
            : base(ApplicationAttributeTypeGroup)
        {
            this._DAO = ApplicationAttributeTypeGroup;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeTypeGroup_DAO.Key
        /// </summary>
        public int Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeTypeGroup_DAO.Description
        /// </summary>
        public string Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeTypeGroup_DAO.ApplicationAttributeTypes
        /// </summary>
        private DAOEventList<ApplicationAttributeType_DAO, IApplicationAttributeType, ApplicationAttributeType> _ApplicationAttributeTypes;

        public override void Refresh()
        {
            base.Refresh();
            _ApplicationAttributeTypes = null;
        }
    }
}