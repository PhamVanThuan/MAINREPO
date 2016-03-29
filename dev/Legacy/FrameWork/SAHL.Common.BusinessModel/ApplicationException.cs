using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationException_DAO
    /// </summary>
    public partial class ApplicationException : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationException_DAO>, IApplicationException
    {
        public ApplicationException(SAHL.Common.BusinessModel.DAO.ApplicationException_DAO ApplicationException)
            : base(ApplicationException)
        {
            this._DAO = ApplicationException;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationException_DAO.Application
        /// </summary>
        public IApplication Application
        {
            get
            {
                if (null == _DAO.Application) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplication, Application_DAO>(_DAO.Application);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Application = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Application = (Application_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationException_DAO.OverRidden
        /// </summary>
        public Boolean OverRidden
        {
            get { return _DAO.OverRidden; }
            set { _DAO.OverRidden = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationException_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationException_DAO.ApplicationExceptionType
        /// </summary>
        public IApplicationExceptionType ApplicationExceptionType
        {
            get
            {
                if (null == _DAO.ApplicationExceptionType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationExceptionType, ApplicationExceptionType_DAO>(_DAO.ApplicationExceptionType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationExceptionType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationExceptionType = (ApplicationExceptionType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}