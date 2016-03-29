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
    public partial class SecurityGroup : IEntityValidation, ISecurityGroup, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.SecurityGroup_DAO _SecurityGroup;

        public SecurityGroup(SAHL.Common.X2.BusinessModel.DAO.SecurityGroup_DAO SecurityGroup)
        {
            this._SecurityGroup = SecurityGroup;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="SecurityGroup_DAO"/></returns>
        public object GetDAOObject()
        {
            return _SecurityGroup;
        }

        /// <summary>
        ///
        /// </summary>
        public Boolean IsDynamic
        {
            get { return _SecurityGroup.IsDynamic; }
            set { _SecurityGroup.IsDynamic = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String Name
        {
            get { return _SecurityGroup.Name; }
            set { _SecurityGroup.Name = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String Description
        {
            get { return _SecurityGroup.Description; }
            set { _SecurityGroup.Description = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _SecurityGroup.ID; }
            set { _SecurityGroup.ID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public IProcess Process
        {
            get
            {
                if (null == _SecurityGroup.Process) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IProcess, Process_DAO>(_SecurityGroup.Process);
                }
            }

            set
            {
                if (value == null)
                {
                    _SecurityGroup.Process = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _SecurityGroup.Process = (Process_DAO)obj.GetDAOObject();
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
                if (null == _SecurityGroup.WorkFlow) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IWorkFlow, WorkFlow_DAO>(_SecurityGroup.WorkFlow);
                }
            }

            set
            {
                if (value == null)
                {
                    _SecurityGroup.WorkFlow = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _SecurityGroup.WorkFlow = (WorkFlow_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
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
                    if (null == _SecurityGroup.Activities)
                        _SecurityGroup.Activities = new List<Activity_DAO>();
                    _Activities = new DAOEventList<Activity_DAO, IActivity, Activity>(_SecurityGroup.Activities);
                    _Activities.BeforeAdd += new EventListHandler(OnActivities_BeforeAdd);
                    _Activities.BeforeRemove += new EventListHandler(OnActivities_BeforeRemove);
                }
                return _Activities;
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
                    if (null == _SecurityGroup.States)
                        _SecurityGroup.States = new List<State_DAO>();
                    _States = new DAOEventList<State_DAO, IState, State>(_SecurityGroup.States);
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
                    if (null == _SecurityGroup.WorkFlows)
                        _SecurityGroup.WorkFlows = new List<WorkFlow_DAO>();
                    _WorkFlows = new DAOEventList<WorkFlow_DAO, IWorkFlow, WorkFlow>(_SecurityGroup.WorkFlows);
                    _WorkFlows.BeforeAdd += new EventListHandler(OnWorkFlows_BeforeAdd);
                    _WorkFlows.BeforeRemove += new EventListHandler(OnWorkFlows_BeforeRemove);
                }
                return _WorkFlows;
            }
        }
    }
}