using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationRoleAttribute_DAO
    /// </summary>
    public partial class ApplicationRoleAttribute : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationRoleAttribute_DAO>, IApplicationRoleAttribute
    {
        public ApplicationRoleAttribute(SAHL.Common.BusinessModel.DAO.ApplicationRoleAttribute_DAO ApplicationRoleAttribute)
            : base(ApplicationRoleAttribute)
        {
            this._DAO = ApplicationRoleAttribute;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationRoleAttribute_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationRoleAttribute_DAO.OfferRole
        /// </summary>
        public IApplicationRole OfferRole
        {
            get
            {
                if (null == _DAO.OfferRole) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationRole, ApplicationRole_DAO>(_DAO.OfferRole);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.OfferRole = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.OfferRole = (ApplicationRole_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationRoleAttribute_DAO.OfferRoleAttributeType
        /// </summary>
        public IApplicationRoleAttributeType OfferRoleAttributeType
        {
            get
            {
                if (null == _DAO.OfferRoleAttributeType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationRoleAttributeType, ApplicationRoleAttributeType_DAO>(_DAO.OfferRoleAttributeType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.OfferRoleAttributeType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.OfferRoleAttributeType = (ApplicationRoleAttributeType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}