using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// CapOfferDetail_DAO is instantiated in order to represent the detailed information regarding the 3 different CAPOffers
    /// which the client is offered.
    /// </summary>
    public partial class CapApplicationDetail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CapApplicationDetail_DAO>, ICapApplicationDetail
    {
        public CapApplicationDetail(SAHL.Common.BusinessModel.DAO.CapApplicationDetail_DAO CapApplicationDetail)
            : base(CapApplicationDetail)
        {
            this._DAO = CapApplicationDetail;
        }

        /// <summary>
        /// The CAP effective rate. This is the rate which the client is being offered to CAP their loan at. This rate will either
        /// be 1%, 2% or 3% above their current rate.
        /// </summary>
        public Double EffectiveRate
        {
            get { return _DAO.EffectiveRate; }
            set { _DAO.EffectiveRate = value; }
        }

        /// <summary>
        /// This is the loan instalment which will be due by the client after the CAP 2 Readvance has been performed.
        /// </summary>
        public Double Payment
        {
            get { return _DAO.Payment; }
            set { _DAO.Payment = value; }
        }

        /// <summary>
        /// This the amount that the client is paying for the CAP 2 product.
        /// </summary>
        public Double Fee
        {
            get { return _DAO.Fee; }
            set { _DAO.Fee = value; }
        }

        /// <summary>
        /// The date on which the CapOffer was accepted.
        /// </summary>
        public DateTime? AcceptanceDate
        {
            get { return _DAO.AcceptanceDate; }
            set { _DAO.AcceptanceDate = value; }
        }

        /// <summary>
        /// The date on which the client decided to not take up the CapOffer.
        /// </summary>
        public DateTime? CapNTUReasonDate
        {
            get { return _DAO.CapNTUReasonDate; }
            set { _DAO.CapNTUReasonDate = value; }
        }

        /// <summary>
        /// The date on which the CapOfferDetail record was last updated.
        /// </summary>
        public DateTime ChangeDate
        {
            get { return _DAO.ChangeDate; }
            set { _DAO.ChangeDate = value; }
        }

        /// <summary>
        /// The UserID of the person who last updated the CapOfferDetail record.
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
        /// The foreign key reference to the Reason table. Each CapOfferDetail record that is NTU'd by the client requires a Reason
        /// for the NTU decision. The CapOfferDetail can only belong to a single Reason.
        /// </summary>
        public ICapNTUReason CapNTUReason
        {
            get
            {
                if (null == _DAO.CapNTUReason) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICapNTUReason, CapNTUReason_DAO>(_DAO.CapNTUReason);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CapNTUReason = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CapNTUReason = (CapNTUReason_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// The foreign key reference to the CapOffer table. Each CapOfferDetail record belongs to a single CapOffer record.
        /// </summary>
        public ICapApplication CapApplication
        {
            get
            {
                if (null == _DAO.CapApplication) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICapApplication, CapApplication_DAO>(_DAO.CapApplication);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CapApplication = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CapApplication = (CapApplication_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// The foreign key reference to the CapStatus table. Each CapOfferDetail record can have only one status which changes
        /// throughout the life of the loan.
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
        /// The foreign key reference to the CapTypeConfigurationDetail table. Each CapOfferDetail record belongs to a
        /// CapTypeConfigurationDetail record. This is dependent on the type of the CapOffer. i.e. The 1%
        /// </summary>
        public ICapTypeConfigurationDetail CapTypeConfigurationDetail
        {
            get
            {
                if (null == _DAO.CapTypeConfigurationDetail) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICapTypeConfigurationDetail, CapTypeConfigurationDetail_DAO>(_DAO.CapTypeConfigurationDetail);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CapTypeConfigurationDetail = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CapTypeConfigurationDetail = (CapTypeConfigurationDetail_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}