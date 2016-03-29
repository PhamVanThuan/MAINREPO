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
    public partial class StateType : IEntityValidation, IStateType, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.StateType_DAO _StateType;

        public StateType(SAHL.Common.X2.BusinessModel.DAO.StateType_DAO StateType)
        {
            this._StateType = StateType;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="StateType_DAO"/></returns>
        public object GetDAOObject()
        {
            return _StateType;
        }

        /// <summary>
        ///
        /// </summary>
        public String Name
        {
            get { return _StateType.Name; }
            set { _StateType.Name = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _StateType.ID; }
            set { _StateType.ID = value; }
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
                    if (null == _StateType.States)
                        _StateType.States = new List<State_DAO>();
                    _States = new DAOEventList<State_DAO, IState, State>(_StateType.States);
                    _States.BeforeAdd += new EventListHandler(OnStates_BeforeAdd);
                    _States.BeforeRemove += new EventListHandler(OnStates_BeforeRemove);
                }
                return _States;
            }
        }
    }
}