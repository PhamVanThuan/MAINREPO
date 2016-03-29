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
    public partial class State : IEntityValidation, IState, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.State_DAO _State;

        public State(SAHL.Common.X2.BusinessModel.DAO.State_DAO State)
        {
            this._State = State;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="State_DAO"/></returns>
        public object GetDAOObject()
        {
            return _State;
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _State.ID; }
            set { _State.ID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String Name
        {
            get { return _State.Name; }
            set { _State.Name = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Boolean ForwardState
        {
            get { return _State.ForwardState; }
            set { _State.ForwardState = value; }
        }

        /// <summary>
        ///
        /// </summary>
        private DAOEventList<Activity_DAO, IActivity, Activity> _NextActivities;

        /// <summary>
        ///
        /// </summary>
        public IEventList<IActivity> NextActivities
        {
            get
            {
                if (null == _NextActivities)
                {
                    if (null == _State.NextActivities)
                        _State.NextActivities = new List<Activity_DAO>();
                    _NextActivities = new DAOEventList<Activity_DAO, IActivity, Activity>(_State.NextActivities);
                    _NextActivities.BeforeAdd += new EventListHandler(OnNextActivities_BeforeAdd);
                    _NextActivities.BeforeRemove += new EventListHandler(OnNextActivities_BeforeRemove);
                }
                return _NextActivities;
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
                    if (null == _State.Activities)
                        _State.Activities = new List<Activity_DAO>();
                    _Activities = new DAOEventList<Activity_DAO, IActivity, Activity>(_State.Activities);
                    _Activities.BeforeAdd += new EventListHandler(OnActivities_BeforeAdd);
                    _Activities.BeforeRemove += new EventListHandler(OnActivities_BeforeRemove);
                }
                return _Activities;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private DAOEventList<Instance_DAO, IInstance, Instance> _Instances;

        /// <summary>
        ///
        /// </summary>
        public IEventList<IInstance> Instances
        {
            get
            {
                if (null == _Instances)
                {
                    if (null == _State.Instances)
                        _State.Instances = new List<Instance_DAO>();
                    _Instances = new DAOEventList<Instance_DAO, IInstance, Instance>(_State.Instances);
                    _Instances.BeforeAdd += new EventListHandler(OnInstances_BeforeAdd);
                    _Instances.BeforeRemove += new EventListHandler(OnInstances_BeforeRemove);
                }
                return _Instances;
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
                    if (null == _State.Logs)
                        _State.Logs = new List<Log_DAO>();
                    _Logs = new DAOEventList<Log_DAO, ILog, Log>(_State.Logs);
                    _Logs.BeforeAdd += new EventListHandler(OnLogs_BeforeAdd);
                    _Logs.BeforeRemove += new EventListHandler(OnLogs_BeforeRemove);
                }
                return _Logs;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private DAOEventList<State_DAO, IState, State> _States;

        /// <summary>
        ///
        /// </summary>
        public IEventList<IState> States
        {
            get
            {
                if (null == _States)
                {
                    if (null == _State.States)
                        _State.States = new List<State_DAO>();
                    _States = new DAOEventList<State_DAO, IState, State>(_State.States);
                    _States.BeforeAdd += new EventListHandler(OnStates_BeforeAdd);
                    _States.BeforeRemove += new EventListHandler(OnStates_BeforeRemove);
                }
                return _States;
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
                    if (null == _State.WorkFlowHistories)
                        _State.WorkFlowHistories = new List<WorkFlowHistory_DAO>();
                    _WorkFlowHistories = new DAOEventList<WorkFlowHistory_DAO, IWorkFlowHistory, WorkFlowHistory>(_State.WorkFlowHistories);
                    _WorkFlowHistories.BeforeAdd += new EventListHandler(OnWorkFlowHistories_BeforeAdd);
                    _WorkFlowHistories.BeforeRemove += new EventListHandler(OnWorkFlowHistories_BeforeRemove);
                }
                return _WorkFlowHistories;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IStateType StateType
        {
            get
            {
                if (null == _State.StateType) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IStateType, StateType_DAO>(_State.StateType);
                }
            }

            set
            {
                if (value == null)
                {
                    _State.StateType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _State.StateType = (StateType_DAO)obj.GetDAOObject();
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
                if (null == _State.WorkFlow) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IWorkFlow, WorkFlow_DAO>(_State.WorkFlow);
                }
            }

            set
            {
                if (value == null)
                {
                    _State.WorkFlow = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _State.WorkFlow = (WorkFlow_DAO)obj.GetDAOObject();
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
                    if (null == _State.SecurityGroups)
                        _State.SecurityGroups = new List<SecurityGroup_DAO>();
                    _SecurityGroups = new DAOEventList<SecurityGroup_DAO, ISecurityGroup, SecurityGroup>(_State.SecurityGroups);
                    _SecurityGroups.BeforeAdd += new EventListHandler(OnSecurityGroups_BeforeAdd);
                    _SecurityGroups.BeforeRemove += new EventListHandler(OnSecurityGroups_BeforeRemove);
                }
                return _SecurityGroups;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private DAOEventList<Form_DAO, IForm, Form> _Forms;

        /// <summary>
        ///
        /// </summary>
        public IEventList<IForm> Forms
        {
            get
            {
                if (null == _Forms)
                {
                    if (null == _State.Forms)
                        _State.Forms = new List<Form_DAO>();
                    _Forms = new DAOEventList<Form_DAO, IForm, Form>(_State.Forms);
                    _Forms.BeforeAdd += new EventListHandler(OnForms_BeforeAdd);
                    _Forms.BeforeRemove += new EventListHandler(OnForms_BeforeRemove);
                }
                return _Forms;
            }
        }
    }
}