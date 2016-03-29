using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ExternalRole_DAO
    /// </summary>
    public partial class ExternalRole : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ExternalRole_DAO>, IExternalRole
    {
        public ExternalRole(SAHL.Common.BusinessModel.DAO.ExternalRole_DAO ExternalRole)
            : base(ExternalRole)
        {
            this._DAO = ExternalRole;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRole_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRole_DAO.GenericKey
        /// </summary>
        public Int32 GenericKey
        {
            get { return _DAO.GenericKey; }
            set { _DAO.GenericKey = value; }
        }

        /// <summary>
        /// The date when the External Role record was last changed.
        /// </summary>
        public DateTime ChangeDate
        {
            get { return _DAO.ChangeDate; }
            set { _DAO.ChangeDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRole_DAO.ExternalRoleType
        /// </summary>
        public IExternalRoleType ExternalRoleType
        {
            get
            {
                if (null == _DAO.ExternalRoleType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IExternalRoleType, ExternalRoleType_DAO>(_DAO.ExternalRoleType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ExternalRoleType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ExternalRoleType = (ExternalRoleType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRole_DAO.GeneralStatus
        /// </summary>
        public IGeneralStatus GeneralStatus
        {
            get
            {
                if (null == _DAO.GeneralStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.GeneralStatus = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRole_DAO.GenericKeyType
        /// </summary>
        public IGenericKeyType GenericKeyType
        {
            get
            {
                if (null == _DAO.GenericKeyType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGenericKeyType, GenericKeyType_DAO>(_DAO.GenericKeyType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.GenericKeyType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.GenericKeyType = (GenericKeyType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalRole_DAO.LegalEntity
        /// </summary>
        public ILegalEntity LegalEntity
        {
            get
            {
                if (null == _DAO.LegalEntity) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.LegalEntity);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.LegalEntity = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.LegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// A collection of external role declaration that are defined for this ExternalRole.
        /// </summary>
        private DAOEventList<ExternalRoleDeclaration_DAO, IExternalRoleDeclaration, ExternalRoleDeclaration> _externalRoleDeclarations;

        /// <summary>
        /// A collection of external role declaration that are defined for this ExternalRole.
        /// </summary>
        public IEventList<IExternalRoleDeclaration> ExternalRoleDeclarations
        {
            get
            {
                if (null == _externalRoleDeclarations)
                {
                    if (null == _DAO.ExternalRoleDeclarations)
                        _DAO.ExternalRoleDeclarations = new List<ExternalRoleDeclaration_DAO>();
                    _externalRoleDeclarations = new DAOEventList<ExternalRoleDeclaration_DAO, IExternalRoleDeclaration, ExternalRoleDeclaration>(_DAO.ExternalRoleDeclarations);
                    _externalRoleDeclarations.BeforeAdd += new EventListHandler(OnExternalRoleDeclarations_BeforeAdd);
                    _externalRoleDeclarations.BeforeRemove += new EventListHandler(OnExternalRoleDeclarations_BeforeRemove);
                    _externalRoleDeclarations.AfterAdd += new EventListHandler(OnExternalRoleDeclarations_AfterAdd);
                    _externalRoleDeclarations.AfterRemove += new EventListHandler(OnExternalRoleDeclarations_AfterRemove);
                }
                return _externalRoleDeclarations;
            }
        }
    }
}