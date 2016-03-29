using System;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class ExternalActivityTarget : IEntityValidation, IExternalActivityTarget, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.ExternalActivityTarget_DAO _ExternalActivityTarget;

        public ExternalActivityTarget(SAHL.Common.X2.BusinessModel.DAO.ExternalActivityTarget_DAO ExternalActivityTarget)
        {
            this._ExternalActivityTarget = ExternalActivityTarget;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="ExternalActivityTarget_DAO"/></returns>
        public object GetDAOObject()
        {
            return _ExternalActivityTarget;
        }

        /// <summary>
        ///
        /// </summary>
        public String Name
        {
            get { return _ExternalActivityTarget.Name; }
            set { _ExternalActivityTarget.Name = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _ExternalActivityTarget.ID; }
            set { _ExternalActivityTarget.ID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        private DAOEventList<Activity_DAO, IActivity, Activity> _Activities;

        /// <summary>
        ///
        /// </summary>
        public IEventList<IActivity> Activities
        {
            get
            {
                if (null == _Activities)
                {
                    if (null == _ExternalActivityTarget.Activities)
                        _ExternalActivityTarget.Activities = new List<Activity_DAO>();
                    _Activities = new DAOEventList<Activity_DAO, IActivity, Activity>(_ExternalActivityTarget.Activities);
                    _Activities.BeforeAdd += new EventListHandler(OnActivities_BeforeAdd);
                    _Activities.BeforeRemove += new EventListHandler(OnActivities_BeforeRemove);
                }
                return _Activities;
            }
        }
    }
}