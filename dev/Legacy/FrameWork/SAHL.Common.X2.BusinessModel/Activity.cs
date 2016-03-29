using System;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class Activity : IEntityValidation, IActivity, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.Activity_DAO _Activity;

        public Activity(SAHL.Common.X2.BusinessModel.DAO.Activity_DAO Activity)
        {
            this._Activity = Activity;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="Activity_DAO"/></returns>
        public object GetDAOObject()
        {
            return _Activity;
        }

        /// <summary>
        ///
        /// </summary>
        public String Name
        {
            get { return _Activity.Name; }
            set { _Activity.Name = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Boolean SplitWorkFlow
        {
            get { return _Activity.SplitWorkFlow; }
            set { _Activity.SplitWorkFlow = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 Priority
        {
            get { return _Activity.Priority; }
            set { _Activity.Priority = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String ActivityMessage
        {
            get { return _Activity.ActivityMessage; }
            set { _Activity.ActivityMessage = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ActivatedByExternalActivity
        {
            get { return _Activity.ActivatedByExternalActivity; }
            set { _Activity.ActivatedByExternalActivity = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String ChainedActivityName
        {
            get { return _Activity.ChainedActivityName; }
            set { _Activity.ChainedActivityName = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _Activity.ID; }
            set { _Activity.ID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        private DAOEventList<InstanceActivitySecurity_DAO, IInstanceActivitySecurity, InstanceActivitySecurity> _InstanceActivitySecurities;

        /// <summary>
        ///
        /// </summary>
        public IEventList<IInstanceActivitySecurity> InstanceActivitySecurities
        {
            get
            {
                if (null == _InstanceActivitySecurities)
                {
                    if (null == _Activity.InstanceActivitySecurities)
                        _Activity.InstanceActivitySecurities = new List<InstanceActivitySecurity_DAO>();
                    _InstanceActivitySecurities = new DAOEventList<InstanceActivitySecurity_DAO, IInstanceActivitySecurity, InstanceActivitySecurity>(_Activity.InstanceActivitySecurities);
                    _InstanceActivitySecurities.BeforeAdd += new EventListHandler(OnInstanceActivitySecurities_BeforeAdd);
                    _InstanceActivitySecurities.BeforeRemove += new EventListHandler(OnInstanceActivitySecurities_BeforeRemove);
                }
                return _InstanceActivitySecurities;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private DAOEventList<Log_DAO, ILog, Log> _Logs;

        /// <summary>
        ///
        /// </summary>
        public IEventList<ILog> Logs
        {
            get
            {
                if (null == _Logs)
                {
                    if (null == _Activity.Logs)
                        _Activity.Logs = new List<Log_DAO>();
                    _Logs = new DAOEventList<Log_DAO, ILog, Log>(_Activity.Logs);
                    _Logs.BeforeAdd += new EventListHandler(OnLogs_BeforeAdd);
                    _Logs.BeforeRemove += new EventListHandler(OnLogs_BeforeRemove);
                }
                return _Logs;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private DAOEventList<StageActivity_DAO, IStageActivity, StageActivity> _StageActivities;

        /// <summary>
        ///
        /// </summary>
        public IEventList<IStageActivity> StageActivities
        {
            get
            {
                if (null == _StageActivities)
                {
                    if (null == _Activity.StageActivities)
                        _Activity.StageActivities = new List<StageActivity_DAO>();
                    _StageActivities = new DAOEventList<StageActivity_DAO, IStageActivity, StageActivity>(_Activity.StageActivities);
                    _StageActivities.BeforeAdd += new EventListHandler(OnStageActivities_BeforeAdd);
                    _StageActivities.BeforeRemove += new EventListHandler(OnStageActivities_BeforeRemove);
                }
                return _StageActivities;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private DAOEventList<WorkFlowActivity_DAO, IWorkFlowActivity, WorkFlowActivity> _WorkFlowActivities;

        /// <summary>
        ///
        /// </summary>
        public IEventList<IWorkFlowActivity> WorkFlowActivities
        {
            get
            {
                if (null == _WorkFlowActivities)
                {
                    if (null == _Activity.WorkFlowActivities)
                        _Activity.WorkFlowActivities = new List<WorkFlowActivity_DAO>();
                    _WorkFlowActivities = new DAOEventList<WorkFlowActivity_DAO, IWorkFlowActivity, WorkFlowActivity>(_Activity.WorkFlowActivities);
                    _WorkFlowActivities.BeforeAdd += new EventListHandler(OnWorkFlowActivities_BeforeAdd);
                    _WorkFlowActivities.BeforeRemove += new EventListHandler(OnWorkFlowActivities_BeforeRemove);
                }
                return _WorkFlowActivities;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private DAOEventList<WorkFlowHistory_DAO, IWorkFlowHistory, WorkFlowHistory> _WorkFlowHistories;

        /// <summary>
        ///
        /// </summary>
        public IEventList<IWorkFlowHistory> WorkFlowHistories
        {
            get
            {
                if (null == _WorkFlowHistories)
                {
                    if (null == _Activity.WorkFlowHistories)
                        _Activity.WorkFlowHistories = new List<WorkFlowHistory_DAO>();
                    _WorkFlowHistories = new DAOEventList<WorkFlowHistory_DAO, IWorkFlowHistory, WorkFlowHistory>(_Activity.WorkFlowHistories);
                    _WorkFlowHistories.BeforeAdd += new EventListHandler(OnWorkFlowHistories_BeforeAdd);
                    _WorkFlowHistories.BeforeRemove += new EventListHandler(OnWorkFlowHistories_BeforeRemove);
                }
                return _WorkFlowHistories;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IActivityType ActivityType
        {
            get
            {
                if (null == _Activity.ActivityType) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IActivityType, ActivityType_DAO>(_Activity.ActivityType);
                }
            }

            set
            {
                if (value == null)
                {
                    _Activity.ActivityType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _Activity.ActivityType = (ActivityType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IExternalActivity ExternalActivity
        {
            get
            {
                if (null == _Activity.ExternalActivity) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IExternalActivity, ExternalActivity_DAO>(_Activity.ExternalActivity);
                }
            }

            set
            {
                if (value == null)
                {
                    _Activity.ExternalActivity = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _Activity.ExternalActivity = (ExternalActivity_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IExternalActivityTarget ExternalActivityTarget
        {
            get
            {
                if (null == _Activity.ExternalActivityTarget) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IExternalActivityTarget, ExternalActivityTarget_DAO>(_Activity.ExternalActivityTarget);
                }
            }

            set
            {
                if (value == null)
                {
                    _Activity.ExternalActivityTarget = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _Activity.ExternalActivityTarget = (ExternalActivityTarget_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IForm Form
        {
            get
            {
                if (null == _Activity.Form) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IForm, Form_DAO>(_Activity.Form);
                }
            }

            set
            {
                if (value == null)
                {
                    _Activity.Form = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _Activity.Form = (Form_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IState NextState
        {
            get
            {
                if (null == _Activity.NextState) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IState, State_DAO>(_Activity.NextState);
                }
            }

            set
            {
                if (value == null)
                {
                    _Activity.NextState = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _Activity.NextState = (State_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IState State
        {
            get
            {
                if (null == _Activity.State) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IState, State_DAO>(_Activity.State);
                }
            }

            set
            {
                if (value == null)
                {
                    _Activity.State = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _Activity.State = (State_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IWorkFlow WorkFlow
        {
            get
            {
                if (null == _Activity.WorkFlow) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IWorkFlow, WorkFlow_DAO>(_Activity.WorkFlow);
                }
            }

            set
            {
                if (value == null)
                {
                    _Activity.WorkFlow = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _Activity.WorkFlow = (WorkFlow_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        private DAOEventList<SecurityGroup_DAO, ISecurityGroup, SecurityGroup> _SecurityGroups;

        /// <summary>
        ///
        /// </summary>
        public IEventList<ISecurityGroup> SecurityGroups
        {
            get
            {
                if (null == _SecurityGroups)
                {
                    if (null == _Activity.SecurityGroups)
                        _Activity.SecurityGroups = new List<SecurityGroup_DAO>();
                    _SecurityGroups = new DAOEventList<SecurityGroup_DAO, ISecurityGroup, SecurityGroup>(_Activity.SecurityGroups);
                    _SecurityGroups.BeforeAdd += new EventListHandler(OnSecurityGroups_BeforeAdd);
                    _SecurityGroups.BeforeRemove += new EventListHandler(OnSecurityGroups_BeforeRemove);
                }
                return _SecurityGroups;
            }
        }
    }
}