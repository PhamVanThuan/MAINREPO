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
    public partial class WorkFlowHistory : IEntityValidation, IWorkFlowHistory, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.WorkFlowHistory_DAO _WorkFlowHistory;

        public WorkFlowHistory(SAHL.Common.X2.BusinessModel.DAO.WorkFlowHistory_DAO WorkFlowHistory)
        {
            this._WorkFlowHistory = WorkFlowHistory;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="WorkFlowHistory_DAO"/></returns>
        public object GetDAOObject()
        {
            return _WorkFlowHistory;
        }

        /// <summary>
        ///
        /// </summary>
        public Int64 InstanceID
        {
            get { return _WorkFlowHistory.InstanceID; }
            set { _WorkFlowHistory.InstanceID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String CreatorADUserName
        {
            get { return _WorkFlowHistory.CreatorADUserName; }
            set { _WorkFlowHistory.CreatorADUserName = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime CreationDate
        {
            get { return _WorkFlowHistory.CreationDate; }
            set { _WorkFlowHistory.CreationDate = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime StateChangeDate
        {
            get { return _WorkFlowHistory.StateChangeDate; }
            set { _WorkFlowHistory.StateChangeDate = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? DeadlineDate
        {
            get { return _WorkFlowHistory.DeadlineDate; }
            set { _WorkFlowHistory.DeadlineDate = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime ActivityDate
        {
            get { return _WorkFlowHistory.ActivityDate; }
            set { _WorkFlowHistory.ActivityDate = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String ADUserName
        {
            get { return _WorkFlowHistory.ADUserName; }
            set { _WorkFlowHistory.ADUserName = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32? Priority
        {
            get { return _WorkFlowHistory.Priority; }
            set { _WorkFlowHistory.Priority = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _WorkFlowHistory.ID; }
            set { _WorkFlowHistory.ID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public IActivity Activity
        {
            get
            {
                if (null == _WorkFlowHistory.Activity) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IActivity, Activity_DAO>(_WorkFlowHistory.Activity);
                }
            }

            set
            {
                if (value == null)
                {
                    _WorkFlowHistory.Activity = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _WorkFlowHistory.Activity = (Activity_DAO)obj.GetDAOObject();
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
                if (null == _WorkFlowHistory.State) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IState, State_DAO>(_WorkFlowHistory.State);
                }
            }

            set
            {
                if (value == null)
                {
                    _WorkFlowHistory.State = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _WorkFlowHistory.State = (State_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}