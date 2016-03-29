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
    public partial class Log : IEntityValidation, ILog, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.Log_DAO _Log;

        public Log(SAHL.Common.X2.BusinessModel.DAO.Log_DAO Log)
        {
            this._Log = Log;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="Log_DAO"/></returns>
        public object GetDAOObject()
        {
            return _Log;
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime Date
        {
            get { return _Log.Date; }
            set { _Log.Date = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32? ProcessID
        {
            get { return _Log.ProcessID; }
            set { _Log.ProcessID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32? WorkFlowID
        {
            get { return _Log.WorkFlowID; }
            set { _Log.WorkFlowID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32? InstanceID
        {
            get { return _Log.InstanceID; }
            set { _Log.InstanceID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String ADUserName
        {
            get { return _Log.ADUserName; }
            set { _Log.ADUserName = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String Message
        {
            get { return _Log.Message; }
            set { _Log.Message = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String StackTrace
        {
            get { return _Log.StackTrace; }
            set { _Log.StackTrace = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _Log.ID; }
            set { _Log.ID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public IActivity Activity
        {
            get
            {
                if (null == _Log.Activity) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IActivity, Activity_DAO>(_Log.Activity);
                }
            }

            set
            {
                if (value == null)
                {
                    _Log.Activity = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _Log.Activity = (Activity_DAO)obj.GetDAOObject();
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
                if (null == _Log.State) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IState, State_DAO>(_Log.State);
                }
            }

            set
            {
                if (value == null)
                {
                    _Log.State = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _Log.State = (State_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}