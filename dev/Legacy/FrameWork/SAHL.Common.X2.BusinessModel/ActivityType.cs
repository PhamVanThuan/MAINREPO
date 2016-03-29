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
    public partial class ActivityType : IEntityValidation, IActivityType, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.ActivityType_DAO _ActivityType;

        public ActivityType(SAHL.Common.X2.BusinessModel.DAO.ActivityType_DAO ActivityType)
        {
            this._ActivityType = ActivityType;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="ActivityType_DAO"/></returns>
        public object GetDAOObject()
        {
            return _ActivityType;
        }

        /// <summary>
        ///
        /// </summary>
        public String Name
        {
            get { return _ActivityType.Name; }
            set { _ActivityType.Name = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _ActivityType.ID; }
            set { _ActivityType.ID = value; }
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
                    if (null == _ActivityType.Activities)
                        _ActivityType.Activities = new List<Activity_DAO>();
                    _Activities = new DAOEventList<Activity_DAO, IActivity, Activity>(_ActivityType.Activities);
                    _Activities.BeforeAdd += new EventListHandler(OnActivities_BeforeAdd);
                    _Activities.BeforeRemove += new EventListHandler(OnActivities_BeforeRemove);
                }
                return _Activities;
            }
        }
    }
}