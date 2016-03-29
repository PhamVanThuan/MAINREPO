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
    /// CapTypeConfigurationDetail_DAO is created in order to store the detailed sales configurations for each of the
    /// CapTypes.
    /// </summary>
    public partial class CapTypeConfigurationDetail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CapTypeConfigurationDetail_DAO>, ICapTypeConfigurationDetail
    {
        public CapTypeConfigurationDetail(SAHL.Common.BusinessModel.DAO.CapTypeConfigurationDetail_DAO CapTypeConfigurationDetail)
            : base(CapTypeConfigurationDetail)
        {
            this._DAO = CapTypeConfigurationDetail;
        }

        /// <summary>
        /// The CAP Base Rate for the CapOffer. This is calculated as JIBAR as of the last rate reset + the margin for the CAP Type.
        /// e.g. For the CAP sales period commencing 18/01/08 and ending 18/04/08, a 2% CAP would have a CAP Base Rate of 13.30%,
        /// which is JIBAR as of the 18/01/08 reset (11.30%) + the 2% margin.
        /// </summary>
        public Double Rate
        {
            get { return _DAO.Rate; }
            set { _DAO.Rate = value; }
        }

        /// <summary>
        /// The status of the CapTypeConfigurationDetail
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
        /// The total premium (FeePremium + AdminPremium)  payable for the CAP product. This is expressed per rand that the
        /// client wishes to CAP
        /// </summary>
        public Double Premium
        {
            get { return _DAO.Premium; }
            set { _DAO.Premium = value; }
        }

        /// <summary>
        /// The Premium Fee payable for the CAP product. This is expressed per rand that the client wishes to CAP.
        /// </summary>
        public Double FeePremium
        {
            get { return _DAO.FeePremium; }
            set { _DAO.FeePremium = value; }
        }

        /// <summary>
        /// The Administration Fee payable for the CAP product. This is expressed per rand that the client wishes to CAP.
        /// </summary>
        public Double FeeAdmin
        {
            get { return _DAO.FeeAdmin; }
            set { _DAO.FeeAdmin = value; }
        }

        /// <summary>
        /// This the strike rate for the CAP, which is related to the trade bought for the CAP.
        /// </summary>
        public Double RateFinance
        {
            get { return _DAO.RateFinance; }
            set { _DAO.RateFinance = value; }
        }

        /// <summary>
        /// The date on which the CapTypeConfigurationDetail records were last changed.
        /// </summary>
        public DateTime? ChangeDate
        {
            get { return _DAO.ChangeDate; }
            set { _DAO.ChangeDate = value; }
        }

        /// <summary>
        /// The UserID of the user who last changed the CapTypeConfigurationDetail records.
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
        /// A foreign key reference to the CapTypeConfigurationDetailKey is stored against the CapOfferDetail record. Each of the
        /// CapOffers will have a 1%,2% and 3% CapOfferDetail record which is then linked back to the Sales Configuration via this
        /// one-to-many relationship.
        /// </summary>
        private DAOEventList<CapApplicationDetail_DAO, ICapApplicationDetail, CapApplicationDetail> _CapApplicationDetails;

        /// <summary>
        /// A foreign key reference to the CapTypeConfigurationDetailKey is stored against the CapOfferDetail record. Each of the
        /// CapOffers will have a 1%,2% and 3% CapOfferDetail record which is then linked back to the Sales Configuration via this
        /// one-to-many relationship.
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
        /// Each CapTypeConfigurationDetail record belongs to a CapType.
        /// </summary>
        public ICapType CapType
        {
            get
            {
                if (null == _DAO.CapType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICapType, CapType_DAO>(_DAO.CapType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CapType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CapType = (CapType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// Each CapTypeConfigurationDetail belongs to a CapTypeConfiguration.
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

        public override void Refresh()
        {
            base.Refresh();
            _CapApplicationDetails = null;
        }
    }
}