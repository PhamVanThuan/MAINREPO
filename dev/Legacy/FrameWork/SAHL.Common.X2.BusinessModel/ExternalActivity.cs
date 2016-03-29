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
    public partial class ExternalActivity : IEntityValidation, IExternalActivity, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.ExternalActivity_DAO _ExternalActivity;

        public ExternalActivity(SAHL.Common.X2.BusinessModel.DAO.ExternalActivity_DAO ExternalActivity)
        {
            this._ExternalActivity = ExternalActivity;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="ExternalActivity_DAO"/></returns>
        public object GetDAOObject()
        {
            return _ExternalActivity;
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 WorkFlowID
        {
            get { return _ExternalActivity.WorkFlowID; }
            set { _ExternalActivity.WorkFlowID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String Name
        {
            get { return _ExternalActivity.Name; }
            set { _ExternalActivity.Name = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String Description
        {
            get { return _ExternalActivity.Description; }
            set { _ExternalActivity.Description = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _ExternalActivity.ID; }
            set { _ExternalActivity.ID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        private DAOEventList<ActiveExternalActivity_DAO, IActiveExternalActivity, ActiveExternalActivity> _ActiveExternalActivities;

        /// <summary>
        ///
        /// </summary>
        public IEventList<IActiveExternalActivity> ActiveExternalActivities
        {
            get
            {
                if (null == _ActiveExternalActivities)
                {
                    if (null == _ExternalActivity.ActiveExternalActivities)
                        _ExternalActivity.ActiveExternalActivities = new List<ActiveExternalActivity_DAO>();
                    _ActiveExternalActivities = new DAOEventList<ActiveExternalActivity_DAO, IActiveExternalActivity, ActiveExternalActivity>(_ExternalActivity.ActiveExternalActivities);
                    _ActiveExternalActivities.BeforeAdd += new EventListHandler(OnActiveExternalActivities_BeforeAdd);
                    _ActiveExternalActivities.BeforeRemove += new EventListHandler(OnActiveExternalActivities_BeforeRemove);
                }
                return _ActiveExternalActivities;
            }
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
                    if (null == _ExternalActivity.Activities)
                        _ExternalActivity.Activities = new List<Activity_DAO>();
                    _Activities = new DAOEventList<Activity_DAO, IActivity, Activity>(_ExternalActivity.Activities);
                    _Activities.BeforeAdd += new EventListHandler(OnActivities_BeforeAdd);
                    _Activities.BeforeRemove += new EventListHandler(OnActivities_BeforeRemove);
                }
                return _Activities;
            }
        }
    }
}