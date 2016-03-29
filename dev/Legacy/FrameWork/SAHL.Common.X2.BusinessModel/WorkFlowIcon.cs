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
    public partial class WorkFlowIcon : IEntityValidation, IWorkFlowIcon, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.WorkFlowIcon_DAO _WorkFlowIcon;

        public WorkFlowIcon(SAHL.Common.X2.BusinessModel.DAO.WorkFlowIcon_DAO WorkFlowIcon)
        {
            this._WorkFlowIcon = WorkFlowIcon;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="WorkFlowIcon_DAO"/></returns>
        public object GetDAOObject()
        {
            return _WorkFlowIcon;
        }

        /// <summary>
        ///
        /// </summary>
        public String Name
        {
            get { return _WorkFlowIcon.Name; }
            set { _WorkFlowIcon.Name = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _WorkFlowIcon.ID; }
            set { _WorkFlowIcon.ID = value; }
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
                    if (null == _WorkFlowIcon.WorkFlows)
                        _WorkFlowIcon.WorkFlows = new List<WorkFlow_DAO>();
                    _WorkFlows = new DAOEventList<WorkFlow_DAO, IWorkFlow, WorkFlow>(_WorkFlowIcon.WorkFlows);
                    _WorkFlows.BeforeAdd += new EventListHandler(OnWorkFlows_BeforeAdd);
                    _WorkFlows.BeforeRemove += new EventListHandler(OnWorkFlows_BeforeRemove);
                }
                return _WorkFlows;
            }
        }
    }
}