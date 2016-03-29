using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ExternalRoleDomicilium_DAO
    /// </summary>
    public partial class ExternalRoleDomicilium : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ExternalRoleDomicilium_DAO>, IExternalRoleDomicilium
    {
        public ExternalRoleDomicilium(SAHL.Common.BusinessModel.DAO.ExternalRoleDomicilium_DAO ExternalRoleDomicilium)
            : base(ExternalRoleDomicilium)
        {
            this._DAO = ExternalRoleDomicilium;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRoleDomicilium_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRoleDomicilium_DAO.LegalEntityDomicilium
        /// </summary>
        public ILegalEntityDomicilium LegalEntityDomicilium
        {
            get
            {
                if (null == _DAO.LegalEntityDomicilium) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILegalEntityDomicilium, LegalEntityDomicilium_DAO>(_DAO.LegalEntityDomicilium);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.LegalEntityDomicilium = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.LegalEntityDomicilium = (LegalEntityDomicilium_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRoleDomicilium_DAO.ExternalRole
        /// </summary>
        public IExternalRole ExternalRole
        {
            get
            {
                if (null == _DAO.ExternalRole) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IExternalRole, ExternalRole_DAO>(_DAO.ExternalRole);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ExternalRole = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ExternalRole = (ExternalRole_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRoleDomicilium_DAO.ADUser
        /// </summary>
        public IADUser ADUser
        {
            get
            {
                if (null == _DAO.ADUser) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IADUser, ADUser_DAO>(_DAO.ADUser);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ADUser = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ADUser = (ADUser_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRoleDomicilium_DAO.ChangeDate
        /// </summary>
        public DateTime? ChangeDate
        {
            get { return _DAO.ChangeDate; }
            set { _DAO.ChangeDate = value; }
        }
    }
}