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
    /// ApplicationInformationQuickCashDetail_DAO is instantiated in order to retrieve the details of the Quick Cash Payments associated
    /// to the Quick Cash Application.
    /// </summary>
    public partial class ApplicationInformationQuickCashDetail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationInformationQuickCashDetail_DAO>, IApplicationInformationQuickCashDetail
    {
        public ApplicationInformationQuickCashDetail(SAHL.Common.BusinessModel.DAO.ApplicationInformationQuickCashDetail_DAO ApplicationInformationQuickCashDetail)
            : base(ApplicationInformationQuickCashDetail)
        {
            this._DAO = ApplicationInformationQuickCashDetail;
        }

        /// <summary>
        /// Primary Key. This is also a foreign key reference to the OfferInformation table.
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// The Interest Rate applicable to the Quick Cash Payment
        /// </summary>
        public Double? InterestRate
        {
            get { return _DAO.InterestRate; }
            set { _DAO.InterestRate = value; }
        }

        /// <summary>
        /// The Requested Amount for the particular Quick Cash Payment.
        /// </summary>
        public Double? RequestedAmount
        {
            get { return _DAO.RequestedAmount; }
            set { _DAO.RequestedAmount = value; }
        }

        /// <summary>
        /// The Rate Configuration which applies to the Quick Cash Payment. This allows you to determine the margin and market
        /// rate for the Quick Cash Payment.
        /// </summary>
        public IRateConfiguration RateConfiguration
        {
            get
            {
                if (null == _DAO.RateConfiguration) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IRateConfiguration, RateConfiguration_DAO>(_DAO.RateConfiguration);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.RateConfiguration = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.RateConfiguration = (RateConfiguration_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// An indicator as to whether the Quick Cash payment has been disbursed.
        /// </summary>
        public Boolean? Disbursed
        {
            get { return _DAO.Disbursed; }
            set { _DAO.Disbursed = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationQuickCashDetail_DAO.QuickCashPaymentType
        /// </summary>
        public IQuickCashPaymentType QuickCashPaymentType
        {
            get
            {
                if (null == _DAO.QuickCashPaymentType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IQuickCashPaymentType, QuickCashPaymentType_DAO>(_DAO.QuickCashPaymentType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.QuickCashPaymentType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.QuickCashPaymentType = (QuickCashPaymentType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// Each of the OfferInformationQuickCashDetail records belong to a single OfferInformationQuickCash key.
        /// </summary>
        public IApplicationInformationQuickCash OfferInformationQuickCash
        {
            get
            {
                if (null == _DAO.OfferInformationQuickCash) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationInformationQuickCash, ApplicationInformationQuickCash_DAO>(_DAO.OfferInformationQuickCash);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.OfferInformationQuickCash = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.OfferInformationQuickCash = (ApplicationInformationQuickCash_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationQuickCashDetail_DAO.ApplicationExpenses
        /// </summary>
        private DAOEventList<ApplicationExpense_DAO, IApplicationExpense, ApplicationExpense> _ApplicationExpenses;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationQuickCashDetail_DAO.ApplicationExpenses
        /// </summary>
        public IEventList<IApplicationExpense> ApplicationExpenses
        {
            get
            {
                if (null == _ApplicationExpenses)
                {
                    if (null == _DAO.ApplicationExpenses)
                        _DAO.ApplicationExpenses = new List<ApplicationExpense_DAO>();
                    _ApplicationExpenses = new DAOEventList<ApplicationExpense_DAO, IApplicationExpense, ApplicationExpense>(_DAO.ApplicationExpenses);
                    _ApplicationExpenses.BeforeAdd += new EventListHandler(OnApplicationExpenses_BeforeAdd);
                    _ApplicationExpenses.BeforeRemove += new EventListHandler(OnApplicationExpenses_BeforeRemove);
                    _ApplicationExpenses.AfterAdd += new EventListHandler(OnApplicationExpenses_AfterAdd);
                    _ApplicationExpenses.AfterRemove += new EventListHandler(OnApplicationExpenses_AfterRemove);
                }
                return _ApplicationExpenses;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _ApplicationExpenses = null;
        }
    }
}