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
    public partial class Process : IEntityValidation, IProcess, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.Process_DAO _Process;

        public Process(SAHL.Common.X2.BusinessModel.DAO.Process_DAO Process)
        {
            this._Process = Process;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="Process_DAO"/></returns>
        public object GetDAOObject()
        {
            return _Process;
        }

        /// <summary>
        ///
        /// </summary>
        public String Name
        {
            get { return _Process.Name; }
            set { _Process.Name = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String Version
        {
            get { return _Process.Version; }
            set { _Process.Version = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime CreateDate
        {
            get { return _Process.CreateDate; }
            set { _Process.CreateDate = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _Process.ID; }
            set { _Process.ID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        private DAOEventList<Process_DAO, IProcess, Process> _Processes;

        /// <summary>
        ///
        /// </summary>
        public IEventList<IProcess> Processes
        {
            get
            {
                if (null == _Processes)
                {
                    if (null == _Process.Processes)
                        _Process.Processes = new List<Process_DAO>();
                    _Processes = new DAOEventList<Process_DAO, IProcess, Process>(_Process.Processes);
                    _Processes.BeforeAdd += new EventListHandler(OnProcesses_BeforeAdd);
                    _Processes.BeforeRemove += new EventListHandler(OnProcesses_BeforeRemove);
                }
                return _Processes;
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
                    if (null == _Process.SecurityGroups)
                        _Process.SecurityGroups = new List<SecurityGroup_DAO>();
                    _SecurityGroups = new DAOEventList<SecurityGroup_DAO, ISecurityGroup, SecurityGroup>(_Process.SecurityGroups);
                    _SecurityGroups.BeforeAdd += new EventListHandler(OnSecurityGroups_BeforeAdd);
                    _SecurityGroups.BeforeRemove += new EventListHandler(OnSecurityGroups_BeforeRemove);
                }
                return _SecurityGroups;
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
                    if (null == _Process.WorkFlows)
                        _Process.WorkFlows = new List<WorkFlow_DAO>();
                    _WorkFlows = new DAOEventList<WorkFlow_DAO, IWorkFlow, WorkFlow>(_Process.WorkFlows);
                    _WorkFlows.BeforeAdd += new EventListHandler(OnWorkFlows_BeforeAdd);
                    _WorkFlows.BeforeRemove += new EventListHandler(OnWorkFlows_BeforeRemove);
                }
                return _WorkFlows;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IProcess ProcessAncestor
        {
            get
            {
                if (null == _Process.ProcessAncestor) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IProcess, Process_DAO>(_Process.ProcessAncestor);
                }
            }

            set
            {
                if (value == null)
                {
                    _Process.ProcessAncestor = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _Process.ProcessAncestor = (Process_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}