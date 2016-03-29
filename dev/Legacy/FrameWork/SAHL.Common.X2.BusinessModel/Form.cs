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
    public partial class Form : IEntityValidation, IForm, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.Form_DAO _Form;

        public Form(SAHL.Common.X2.BusinessModel.DAO.Form_DAO Form)
        {
            this._Form = Form;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="Form_DAO"/></returns>
        public object GetDAOObject()
        {
            return _Form;
        }

        /// <summary>
        ///
        /// </summary>
        public String Name
        {
            get { return _Form.Name; }
            set { _Form.Name = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String Description
        {
            get { return _Form.Description; }
            set { _Form.Description = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _Form.ID; }
            set { _Form.ID = value; }
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
                    if (null == _Form.Activities)
                        _Form.Activities = new List<Activity_DAO>();
                    _Activities = new DAOEventList<Activity_DAO, IActivity, Activity>(_Form.Activities);
                    _Activities.BeforeAdd += new EventListHandler(OnActivities_BeforeAdd);
                    _Activities.BeforeRemove += new EventListHandler(OnActivities_BeforeRemove);
                }
                return _Activities;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IWorkFlow WorkFlow
        {
            get
            {
                if (null == _Form.WorkFlow) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IWorkFlow, WorkFlow_DAO>(_Form.WorkFlow);
                }
            }

            set
            {
                if (value == null)
                {
                    _Form.WorkFlow = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _Form.WorkFlow = (WorkFlow_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
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
                    if (null == _Form.States)
                        _Form.States = new List<State_DAO>();
                    _States = new DAOEventList<State_DAO, IState, State>(_Form.States);
                    _States.BeforeAdd += new EventListHandler(OnStates_BeforeAdd);
                    _States.BeforeRemove += new EventListHandler(OnStates_BeforeRemove);
                }
                return _States;
            }
        }
    }
}