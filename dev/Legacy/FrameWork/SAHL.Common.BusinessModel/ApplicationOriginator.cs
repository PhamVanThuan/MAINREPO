using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationOriginator_DAO
    /// </summary>
    public partial class ApplicationOriginator : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationOriginator_DAO>, IApplicationOriginator
    {
        public ApplicationOriginator(SAHL.Common.BusinessModel.DAO.ApplicationOriginator_DAO ApplicationOriginator)
            : base(ApplicationOriginator)
        {
            this._DAO = ApplicationOriginator;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationOriginator_DAO.Contact
        /// </summary>
        public String Contact
        {
            get { return _DAO.Contact; }
            set { _DAO.Contact = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationOriginator_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationOriginator_DAO.GeneralStatus
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
        /// SAHL.Common.BusinessModel.DAO.ApplicationOriginator_DAO.LegalEntity
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
        /// SAHL.Common.BusinessModel.DAO.ApplicationOriginator_DAO.OriginationSource
        /// </summary>
        public IOriginationSource OriginationSource
        {
            get
            {
                if (null == _DAO.OriginationSource) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IOriginationSource, OriginationSource_DAO>(_DAO.OriginationSource);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.OriginationSource = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.OriginationSource = (OriginationSource_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}