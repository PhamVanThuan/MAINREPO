using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationExceptionTypeGroup_DAO
    /// </summary>
    public partial class ApplicationExceptionTypeGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationExceptionTypeGroup_DAO>, IApplicationExceptionTypeGroup
    {
        public ApplicationExceptionTypeGroup(SAHL.Common.BusinessModel.DAO.ApplicationExceptionTypeGroup_DAO ApplicationExceptionTypeGroup)
            : base(ApplicationExceptionTypeGroup)
        {
            this._DAO = ApplicationExceptionTypeGroup;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExceptionTypeGroup_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExceptionTypeGroup_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExceptionTypeGroup_DAO.ApplicationExceptionTypes
        /// </summary>
        private DAOEventList<ApplicationExceptionType_DAO, IApplicationExceptionType, ApplicationExceptionType> _ApplicationExceptionTypes;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExceptionTypeGroup_DAO.ApplicationExceptionTypes
        /// </summary>
        public IEventList<IApplicationExceptionType> ApplicationExceptionTypes
        {
            get
            {
                if (null == _ApplicationExceptionTypes)
                {
                    if (null == _DAO.ApplicationExceptionTypes)
                        _DAO.ApplicationExceptionTypes = new List<ApplicationExceptionType_DAO>();
                    _ApplicationExceptionTypes = new DAOEventList<ApplicationExceptionType_DAO, IApplicationExceptionType, ApplicationExceptionType>(_DAO.ApplicationExceptionTypes);
                    _ApplicationExceptionTypes.BeforeAdd += new EventListHandler(OnApplicationExceptionTypes_BeforeAdd);
                    _ApplicationExceptionTypes.BeforeRemove += new EventListHandler(OnApplicationExceptionTypes_BeforeRemove);
                    _ApplicationExceptionTypes.AfterAdd += new EventListHandler(OnApplicationExceptionTypes_AfterAdd);
                    _ApplicationExceptionTypes.AfterRemove += new EventListHandler(OnApplicationExceptionTypes_AfterRemove);
                }
                return _ApplicationExceptionTypes;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _ApplicationExceptionTypes = null;
        }
    }
}