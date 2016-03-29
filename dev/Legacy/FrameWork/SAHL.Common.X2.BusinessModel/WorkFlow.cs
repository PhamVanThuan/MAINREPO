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
    public partial class WorkFlow : IEntityValidation, IWorkFlow, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.WorkFlow_DAO _WorkFlow;

        public WorkFlow(SAHL.Common.X2.BusinessModel.DAO.WorkFlow_DAO WorkFlow)
        {
            this._WorkFlow = WorkFlow;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="WorkFlow_DAO"/></returns>
        public object GetDAOObject()
        {
            return _WorkFlow;
        }

        /// <summary>
        ///
        /// </summary>
        public String Name
        {
            get { return _WorkFlow.Name; }
            set { _WorkFlow.Name = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime CreateDate
        {
            get { return _WorkFlow.CreateDate; }
            set { _WorkFlow.CreateDate = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String StorageTable
        {
            get { return _WorkFlow.StorageTable; }
            set { _WorkFlow.StorageTable = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String StorageKey
        {
            get { return _WorkFlow.StorageKey; }
            set { _WorkFlow.StorageKey = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 GenericKeyTypeKey
        {
            get { return _WorkFlow.GenericKeyTypeKey; }
            set { _WorkFlow.GenericKeyTypeKey = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String DefaultSubject
        {
            get { return _WorkFlow.DefaultSubject; }
            set { _WorkFlow.DefaultSubject = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _WorkFlow.ID; }
            set { _WorkFlow.ID = value; }
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
                    if (null == _WorkFlow.Activities)
                        _WorkFlow.Activities = new List<Activity_DAO>();
                    _Activities = new DAOEventList<Activity_DAO, IActivity, Activity>(_WorkFlow.Activities);
                    _Activities.BeforeAdd += new EventListHandler(OnActivities_BeforeAdd);
                    _Activities.BeforeRemove += new EventListHandler(OnActivities_BeforeRemove);
                }
                return _Activities;
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
                    if (null == _WorkFlow.Forms)
                        _WorkFlow.Forms = new List<Form_DAO>();
                    _Forms = new DAOEventList<Form_DAO, IForm, Form>(_WorkFlow.Forms);
                    _Forms.BeforeAdd += new EventListHandler(OnForms_BeforeAdd);
                    _Forms.BeforeRemove += new EventListHandler(OnForms_BeforeRemove);
                }
                return _Forms;
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
                    if (null == _WorkFlow.Instances)
                        _WorkFlow.Instances = new List<Instance_DAO>();
                    _Instances = new DAOEventList<Instance_DAO, IInstance, Instance>(_WorkFlow.Instances);
                    _Instances.BeforeAdd += new EventListHandler(OnInstances_BeforeAdd);
                    _Instances.BeforeRemove += new EventListHandler(OnInstances_BeforeRemove);
                }
                return _Instances;
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
                    if (null == _WorkFlow.SecurityGroups)
                        _WorkFlow.SecurityGroups = new List<SecurityGroup_DAO>();
                    _SecurityGroups = new DAOEventList<SecurityGroup_DAO, ISecurityGroup, SecurityGroup>(_WorkFlow.SecurityGroups);
                    _SecurityGroups.BeforeAdd += new EventListHandler(OnSecurityGroups_BeforeAdd);
                    _SecurityGroups.BeforeRemove += new EventListHandler(OnSecurityGroups_BeforeRemove);
                }
                return _SecurityGroups;
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
                    if (null == _WorkFlow.States)
                        _WorkFlow.States = new List<State_DAO>();
                    _States = new DAOEventList<State_DAO, IState, State>(_WorkFlow.States);
                    _States.BeforeAdd += new EventListHandler(OnStates_BeforeAdd);
                    _States.BeforeRemove += new EventListHandler(OnStates_BeforeRemove);
                }
                return _States;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private DAOEventList<WorkFlow_DAO, IWorkFlow, WorkFlow> _WorkFlows;

        /// <summary>
        ///
        /// </summary>
        public IEventList<IWorkFlow> WorkFlows
        {
            get
            {
                if (null == _WorkFlows)
                {
                    if (null == _WorkFlow.WorkFlows)
                        _WorkFlow.WorkFlows = new List<WorkFlow_DAO>();
                    _WorkFlows = new DAOEventList<WorkFlow_DAO, IWorkFlow, WorkFlow>(_WorkFlow.WorkFlows);
                    _WorkFlows.BeforeAdd += new EventListHandler(OnWorkFlows_BeforeAdd);
                    _WorkFlows.BeforeRemove += new EventListHandler(OnWorkFlows_BeforeRemove);
                }
                return _WorkFlows;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private DAOEventList<WorkFlowActivity_DAO, IWorkFlowActivity, WorkFlowActivity> _NextWorkFlowActivities;

        /// <summary>
        ///
        /// </summary>
        public IEventList<IWorkFlowActivity> NextWorkFlowActivities
        {
            get
            {
                if (null == _NextWorkFlowActivities)
                {
                    if (null == _WorkFlow.NextWorkFlowActivities)
                        _WorkFlow.NextWorkFlowActivities = new List<WorkFlowActivity_DAO>();
                    _NextWorkFlowActivities = new DAOEventList<WorkFlowActivity_DAO, IWorkFlowActivity, WorkFlowActivity>(_WorkFlow.NextWorkFlowActivities);
                    _NextWorkFlowActivities.BeforeAdd += new EventListHandler(OnNextWorkFlowActivities_BeforeAdd);
                    _NextWorkFlowActivities.BeforeRemove += new EventListHandler(OnNextWorkFlowActivities_BeforeRemove);
                }
                return _NextWorkFlowActivities;
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
                    if (null == _WorkFlow.WorkFlowActivities)
                        _WorkFlow.WorkFlowActivities = new List<WorkFlowActivity_DAO>();
                    _WorkFlowActivities = new DAOEventList<WorkFlowActivity_DAO, IWorkFlowActivity, WorkFlowActivity>(_WorkFlow.WorkFlowActivities);
                    _WorkFlowActivities.BeforeAdd += new EventListHandler(OnWorkFlowActivities_BeforeAdd);
                    _WorkFlowActivities.BeforeRemove += new EventListHandler(OnWorkFlowActivities_BeforeRemove);
                }
                return _WorkFlowActivities;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IProcess Process
        {
            get
            {
                if (null == _WorkFlow.Process) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IProcess, Process_DAO>(_WorkFlow.Process);
                }
            }

            set
            {
                if (value == null)
                {
                    _WorkFlow.Process = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _WorkFlow.Process = (Process_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IWorkFlow WorkFlowAncestor
        {
            get
            {
                if (null == _WorkFlow.WorkFlowAncestor) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IWorkFlow, WorkFlow_DAO>(_WorkFlow.WorkFlowAncestor);
                }
            }

            set
            {
                if (value == null)
                {
                    _WorkFlow.WorkFlowAncestor = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _WorkFlow.WorkFlowAncestor = (WorkFlow_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IWorkFlowIcon WorkFlowIcon
        {
            get
            {
                if (null == _WorkFlow.WorkFlowIcon) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IWorkFlowIcon, WorkFlowIcon_DAO>(_WorkFlow.WorkFlowIcon);
                }
            }

            set
            {
                if (value == null)
                {
                    _WorkFlow.WorkFlowIcon = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _WorkFlow.WorkFlowIcon = (WorkFlowIcon_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}