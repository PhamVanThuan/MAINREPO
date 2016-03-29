using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationExceptionType_DAO
    /// </summary>
    public partial class ApplicationExceptionType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationExceptionType_DAO>, IApplicationExceptionType
    {
        public ApplicationExceptionType(SAHL.Common.BusinessModel.DAO.ApplicationExceptionType_DAO ApplicationExceptionType)
            : base(ApplicationExceptionType)
        {
            this._DAO = ApplicationExceptionType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExceptionType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExceptionType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExceptionType_DAO.ApplicationExceptionTypeGroup
        /// </summary>
        public IApplicationExceptionTypeGroup ApplicationExceptionTypeGroup
        {
            get
            {
                if (null == _DAO.ApplicationExceptionTypeGroup) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationExceptionTypeGroup, ApplicationExceptionTypeGroup_DAO>(_DAO.ApplicationExceptionTypeGroup);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationExceptionTypeGroup = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationExceptionTypeGroup = (ApplicationExceptionTypeGroup_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}