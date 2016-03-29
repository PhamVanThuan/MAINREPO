using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationMailingAddress_DAO
    /// </summary>
    public partial class ApplicationMailingAddress : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationMailingAddress_DAO>, IApplicationMailingAddress
    {
        public ApplicationMailingAddress(SAHL.Common.BusinessModel.DAO.ApplicationMailingAddress_DAO ApplicationMailingAddress)
            : base(ApplicationMailingAddress)
        {
            this._DAO = ApplicationMailingAddress;
        }

        /// <summary>
        /// The Mailing Address is associated to a particular application. This relationship is defined in the OfferMailingAddress
        /// table where the Offer.OfferKey = OfferMailingAddress.OfferKey.
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
        /// Each Address is associated an AddressKey. The Address details are retrieved from the Address table based on this key.
        /// </summary>
        public IAddress Address
        {
            get
            {
                if (null == _DAO.Address) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAddress, Address_DAO>(_DAO.Address);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Address = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Address = (Address_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// An indicator as to whether the client would like to receive their Loan Statement electronically.
        /// </summary>
        public Boolean OnlineStatement
        {
            get { return _DAO.OnlineStatement; }
            set { _DAO.OnlineStatement = value; }
        }

        /// <summary>
        /// The Electronic Format they would like to receive their Loan Statement in.
        /// </summary>
        public IOnlineStatementFormat OnlineStatementFormat
        {
            get
            {
                if (null == _DAO.OnlineStatementFormat) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IOnlineStatementFormat, OnlineStatementFormat_DAO>(_DAO.OnlineStatementFormat);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.OnlineStatementFormat = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.OnlineStatementFormat = (OnlineStatementFormat_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// This determines the Language preference correspondence sent to the client.
        /// </summary>
        public ILanguage Language
        {
            get
            {
                if (null == _DAO.Language) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILanguage, Language_DAO>(_DAO.Language);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Language = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Language = (Language_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationMailingAddress_DAO.LegalEntity
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
        /// SAHL.Common.BusinessModel.DAO.ApplicationMailingAddress_DAO.CorrespondenceMedium
        /// </summary>
        public ICorrespondenceMedium CorrespondenceMedium
        {
            get
            {
                if (null == _DAO.CorrespondenceMedium) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICorrespondenceMedium, CorrespondenceMedium_DAO>(_DAO.CorrespondenceMedium);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CorrespondenceMedium = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CorrespondenceMedium = (CorrespondenceMedium_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}