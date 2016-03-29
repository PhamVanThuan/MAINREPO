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
    /// CapTypeConfiguration_DAO is used when creating a new CAP Sales Configuration for a CAP Selling Period.
    /// </summary>
    public partial class CapTypeConfiguration : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CapTypeConfiguration_DAO>, ICapTypeConfiguration
    {
        public CapTypeConfiguration(SAHL.Common.BusinessModel.DAO.CapTypeConfiguration_DAO CapTypeConfiguration)
            : base(CapTypeConfiguration)
        {
            this._DAO = CapTypeConfiguration;
        }

        /// <summary>
        /// The date on which the CAP Selling Period begins.
        /// </summary>
        public DateTime ApplicationStartDate
        {
            get { return _DAO.ApplicationStartDate; }
            set { _DAO.ApplicationStartDate = value; }
        }

        /// <summary>
        /// The date on which the CAP Selling Period ends.
        /// </summary>
        public DateTime ApplicationEndDate
        {
            get { return _DAO.ApplicationEndDate; }
            set { _DAO.ApplicationEndDate = value; }
        }

        /// <summary>
        /// The status of the Sales Configuration
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
        /// The date on which the CAP sold will become effective. This is on the next reset date the client reaches after
        /// accepting the CAP Offer.
        /// </summary>
        public DateTime CapEffectiveDate
        {
            get { return _DAO.CapEffectiveDate; }
            set { _DAO.CapEffectiveDate = value; }
        }

        /// <summary>
        /// The Date on which the CAP ends. This is currently set at 24 months after the CapEffectiveDate.
        /// </summary>
        public DateTime CapClosureDate
        {
            get { return _DAO.CapClosureDate; }
            set { _DAO.CapClosureDate = value; }
        }

        /// <summary>
        /// The foreign key reference to the ResetConfiguration table, where the details regarding the next reset dates are stored.
        /// Each CapTypeConfiguration belongs to a single ResetConfiguration that determines whether the CAP is sold to 21st or 18th
        /// reset clients.
        /// </summary>
        public IResetConfiguration ResetConfiguration
        {
            get
            {
                if (null == _DAO.ResetConfiguration) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IResetConfiguration, ResetConfiguration_DAO>(_DAO.ResetConfiguration);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ResetConfiguration = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ResetConfiguration = (ResetConfiguration_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// The ResetDate which is applicable for the Cap Sales Configuration.
        /// </summary>
        public DateTime ResetDate
        {
            get { return _DAO.ResetDate; }
            set { _DAO.ResetDate = value; }
        }

        /// <summary>
        /// The term of the CAP. Currently this is 24 months.
        /// </summary>
        public Int32 Term
        {
            get { return _DAO.Term; }
            set { _DAO.Term = value; }
        }

        /// <summary>
        /// The date on which the CAP Configuration records were last changed.
        /// </summary>
        public DateTime? ChangeDate
        {
            get { return _DAO.ChangeDate; }
            set { _DAO.ChangeDate = value; }
        }

        /// <summary>
        /// The UserID of the person who last updated the CAP Configuration records.
        /// </summary>
        public String UserID
        {
            get { return _DAO.UserID; }
            set { _DAO.UserID = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapTypeConfiguration_DAO.NACQDiscount
        /// </summary>
        public Double? NACQDiscount
        {
            get { return _DAO.NACQDiscount; }
            set { _DAO.NACQDiscount = value; }
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
        /// Each CapTypeConfiguration has many detail records in the CapTypeConfigurationDetail, where the individual admin fees/premiums
        /// per CapType for the sales configuration is stored.
        /// </summary>
        private DAOEventList<CapTypeConfigurationDetail_DAO, ICapTypeConfigurationDetail, CapTypeConfigurationDetail> _CapTypeConfigurationDetails;

        /// <summary>
        /// Each CapTypeConfiguration has many detail records in the CapTypeConfigurationDetail, where the individual admin fees/premiums
        /// per CapType for the sales configuration is stored.
        /// </summary>
        public IEventList<ICapTypeConfigurationDetail> CapTypeConfigurationDetails
        {
            get
            {
                if (null == _CapTypeConfigurationDetails)
                {
                    if (null == _DAO.CapTypeConfigurationDetails)
                        _DAO.CapTypeConfigurationDetails = new List<CapTypeConfigurationDetail_DAO>();
                    _CapTypeConfigurationDetails = new DAOEventList<CapTypeConfigurationDetail_DAO, ICapTypeConfigurationDetail, CapTypeConfigurationDetail>(_DAO.CapTypeConfigurationDetails);
                    _CapTypeConfigurationDetails.BeforeAdd += new EventListHandler(OnCapTypeConfigurationDetails_BeforeAdd);
                    _CapTypeConfigurationDetails.BeforeRemove += new EventListHandler(OnCapTypeConfigurationDetails_BeforeRemove);
                    _CapTypeConfigurationDetails.AfterAdd += new EventListHandler(OnCapTypeConfigurationDetails_AfterAdd);
                    _CapTypeConfigurationDetails.AfterRemove += new EventListHandler(OnCapTypeConfigurationDetails_AfterRemove);
                }
                return _CapTypeConfigurationDetails;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _CapTypeConfigurationDetails = null;
        }
    }
}