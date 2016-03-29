using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CallingContext_DAO
    /// </summary>
    public partial class CallingContext : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CallingContext_DAO>, ICallingContext
    {
        public CallingContext(SAHL.Common.BusinessModel.DAO.CallingContext_DAO CallingContext)
            : base(CallingContext)
        {
            this._DAO = CallingContext;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CallingContext_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CallingContext_DAO.CallingContextType
        /// </summary>
        public ICallingContextType CallingContextType
        {
            get
            {
                if (null == _DAO.CallingContextType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICallingContextType, CallingContextType_DAO>(_DAO.CallingContextType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CallingContextType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CallingContextType = (CallingContextType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CallingContext_DAO.CallingProcess
        /// </summary>
        public String CallingProcess
        {
            get { return _DAO.CallingProcess; }
            set { _DAO.CallingProcess = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CallingContext_DAO.CallingMethod
        /// </summary>
        public String CallingMethod
        {
            get { return _DAO.CallingMethod; }
            set { _DAO.CallingMethod = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CallingContext_DAO.CallingState
        /// </summary>
        public String CallingState
        {
            get { return _DAO.CallingState; }
            set { _DAO.CallingState = value; }
        }
    }
}