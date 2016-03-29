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
    /// CapOffer_DAO is instantiated in order to represent a CapOffer in the system, where a client will be offered the CAP 2 product.
    /// </summary>
    public partial class CapApplication : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CapApplication_DAO>, ICapApplication
    {
        public CapApplication(SAHL.Common.BusinessModel.DAO.CapApplication_DAO CapApplication)
            : base(CapApplication)
        {
            this._DAO = CapApplication;
        }

        /// <summary>
        /// The remaining instalments on the client's loan at the time of the CapOffer being calculated.
        /// </summary>
        public Int32 RemainingInstallments
        {
            get { return _DAO.RemainingInstallments; }
            set { _DAO.RemainingInstallments = value; }
        }

        /// <summary>
        /// The outstanding balance on the client's loan at the time of the CapOffer being calculated.
        /// </summary>
        public Double CurrentBalance
        {
            get { return _DAO.CurrentBalance; }
            set { _DAO.CurrentBalance = value; }
        }

        /// <summary>
        /// The current instalment due on the client's loan at the time of the CapOffer being calculated.
        /// </summary>
        public Double CurrentInstallment
        {
            get { return _DAO.CurrentInstallment; }
            set { _DAO.CurrentInstallment = value; }
        }

        /// <summary>
        /// The loan link rate at the time of the CapOffer being calculated.
        /// </summary>
        public Double LinkRate
        {
            get { return _DAO.LinkRate; }
            set { _DAO.LinkRate = value; }
        }

        /// <summary>
        /// The date on which the CapOffer was calculated.
        /// </summary>
        public DateTime ApplicationDate
        {
            get { return _DAO.ApplicationDate; }
            set { _DAO.ApplicationDate = value; }
        }

        /// <summary>
        /// An indicator as to whether the CAP is forming part of a promotion given to the client. In order to defend cancellations
        /// clients are offered a free 3% CAP.
        /// </summary>
        public Boolean? Promotion
        {
            get { return _DAO.Promotion; }
            set { _DAO.Promotion = value; }
        }

        /// <summary>
        /// The date on which the CapOffer was last changed.
        /// </summary>
        public DateTime ChangeDate
        {
            get { return _DAO.ChangeDate; }
            set { _DAO.ChangeDate = value; }
        }

        /// <summary>
        /// The UserID of the person who last changed the CapOffer.
        /// </summary>
        public String UserID
        {
            get { return _DAO.UserID; }
            set { _DAO.UserID = value; }
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
        /// CapOffer_DAO has a one-to-many relationship to the CapOfferDetail_DAO. Each CapOffer record has many CapOfferDetail
        /// records in the database, one for each of the 1%/2%/3% applications made available to the client.
        /// </summary>
        private DAOEventList<CapApplicationDetail_DAO, ICapApplicationDetail, CapApplicationDetail> _CapApplicationDetails;

        /// <summary>
        /// CapOffer_DAO has a one-to-many relationship to the CapOfferDetail_DAO. Each CapOffer record has many CapOfferDetail
        /// records in the database, one for each of the 1%/2%/3% applications made available to the client.
        /// </summary>
        public IEventList<ICapApplicationDetail> CapApplicationDetails
        {
            get
            {
                if (null == _CapApplicationDetails)
                {
                    if (null == _DAO.CapApplicationDetails)
                        _DAO.CapApplicationDetails = new List<CapApplicationDetail_DAO>();
                    _CapApplicationDetails = new DAOEventList<CapApplicationDetail_DAO, ICapApplicationDetail, CapApplicationDetail>(_DAO.CapApplicationDetails);
                    _CapApplicationDetails.BeforeAdd += new EventListHandler(OnCapApplicationDetails_BeforeAdd);
                    _CapApplicationDetails.BeforeRemove += new EventListHandler(OnCapApplicationDetails_BeforeRemove);
                    _CapApplicationDetails.AfterAdd += new EventListHandler(OnCapApplicationDetails_AfterAdd);
                    _CapApplicationDetails.AfterRemove += new EventListHandler(OnCapApplicationDetails_AfterRemove);
                }
                return _CapApplicationDetails;
            }
        }

        /// <summary>
        /// The foreign key reference to the Account table. Each CapOffer can only belong to one Account.
        /// </summary>
        public IAccount Account
        {
            get
            {
                if (null == _DAO.Account) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAccount, Account_DAO>(_DAO.Account);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Account = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Account = (Account_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// The foreign key reference to the Broker table. Each CapOffer can only belong to a single Broker at a time. This broker can change
        /// throughout the CapOffer process.
        /// </summary>
        public IBroker Broker
        {
            get
            {
                if (null == _DAO.Broker) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IBroker, Broker_DAO>(_DAO.Broker);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Broker = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Broker = (Broker_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// The foreign key reference to the CapStatus table. Each CapOffer can only belong to a single CapStatus, which changes
        /// throughout the life of the CapOffer.
        /// </summary>
        public ICapStatus CapStatus
        {
            get
            {
                if (null == _DAO.CapStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICapStatus, CapStatus_DAO>(_DAO.CapStatus);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CapStatus = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CapStatus = (CapStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// The foreign key reference to the CapTypeConfiguration table. Each CapOffer can only belong to a single CapTypeConfiguration
        /// where information regarding the sales configuration for the CAP product is maintained.
        /// </summary>
        public ICapTypeConfiguration CapTypeConfiguration
        {
            get
            {
                if (null == _DAO.CapTypeConfiguration) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICapTypeConfiguration, CapTypeConfiguration_DAO>(_DAO.CapTypeConfiguration);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CapTypeConfiguration = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CapTypeConfiguration = (CapTypeConfiguration_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapApplication_DAO.CAPPaymentOption
        /// </summary>
        public ICapPaymentOption CAPPaymentOption
        {
            get
            {
                if (null == _DAO.CAPPaymentOption) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICapPaymentOption, CapPaymentOption_DAO>(_DAO.CAPPaymentOption);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CAPPaymentOption = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CAPPaymentOption = (CapPaymentOption_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _CapApplicationDetails = null;
        }
    }
}