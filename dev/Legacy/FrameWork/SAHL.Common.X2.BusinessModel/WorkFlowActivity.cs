using System;
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
    public partial class WorkFlowActivity : IEntityValidation, IWorkFlowActivity, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.WorkFlowActivity_DAO _WorkFlowActivity;

        public WorkFlowActivity(SAHL.Common.X2.BusinessModel.DAO.WorkFlowActivity_DAO WorkFlowActivity)
        {
            this._WorkFlowActivity = WorkFlowActivity;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="WorkFlowActivity_DAO"/></returns>
        public object GetDAOObject()
        {
            return _WorkFlowActivity;
        }

        /// <summary>
        ///
        /// </summary>
        public String Name
        {
            get { return _WorkFlowActivity.Name; }
            set { _WorkFlowActivity.Name = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32? StateID
        {
            get { return _WorkFlowActivity.StateID; }
            set { _WorkFlowActivity.StateID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _WorkFlowActivity.ID; }
            set { _WorkFlowActivity.ID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public IActivity NextActivity
        {
            get
            {
                if (null == _WorkFlowActivity.NextActivity) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IActivity, Activity_DAO>(_WorkFlowActivity.NextActivity);
                }
            }

            set
            {
                if (value == null)
                {
                    _WorkFlowActivity.NextActivity = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _WorkFlowActivity.NextActivity = (Activity_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IWorkFlow NextWorkFlow
        {
            get
            {
                if (null == _WorkFlowActivity.NextWorkFlow) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IWorkFlow, WorkFlow_DAO>(_WorkFlowActivity.NextWorkFlow);
                }
            }

            set
            {
                if (value == null)
                {
                    _WorkFlowActivity.NextWorkFlow = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _WorkFlowActivity.NextWorkFlow = (WorkFlow_DAO)obj.GetDAOObject();
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
                if (null == _WorkFlowActivity.WorkFlow) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IWorkFlow, WorkFlow_DAO>(_WorkFlowActivity.WorkFlow);
                }
            }

            set
            {
                if (value == null)
                {
                    _WorkFlowActivity.WorkFlow = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _WorkFlowActivity.WorkFlow = (WorkFlow_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}