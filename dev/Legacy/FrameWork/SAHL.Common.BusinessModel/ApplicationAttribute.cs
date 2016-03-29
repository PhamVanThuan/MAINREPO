using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Application attributes.
    /// </summary>
    public partial class ApplicationAttribute : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationAttribute_DAO>, IApplicationAttribute
    {
        public ApplicationAttribute(SAHL.Common.BusinessModel.DAO.ApplicationAttribute_DAO ApplicationAttribute)
            : base(ApplicationAttribute)
        {
            this._DAO = ApplicationAttribute;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttribute_DAO.Application
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
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttribute_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttribute_DAO.ApplicationAttributeType
        /// </summary>
        public IApplicationAttributeType ApplicationAttributeType
        {
            get
            {
                if (null == _DAO.ApplicationAttributeType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationAttributeType, ApplicationAttributeType_DAO>(_DAO.ApplicationAttributeType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationAttributeType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationAttributeType = (ApplicationAttributeType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}