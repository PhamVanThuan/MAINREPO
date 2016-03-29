using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ApplicationInformationQuickCashDetail_DAO is instantiated in order to retrieve the details of the Quick Cash Payments associated
    /// to the Quick Cash Application.
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("OfferInformationQuickCashDetail", Schema = "dbo", Lazy = true)]
    public partial class ApplicationInformationQuickCashDetail_DAO : DB_2AM<ApplicationInformationQuickCashDetail_DAO>
    {
        private double? _interestRate;

        private double? _requestedAmount;

        private bool? _disbursed;

        private RateConfiguration_DAO _rateConfiguration;

        private QuickCashPaymentType_DAO _quickCashPaymentType;

        private ApplicationInformationQuickCash_DAO _offerInformationQuickCash;

        private int _key;

        private IList<ApplicationExpense_DAO> _applicationExpenses;

        //private ApplicationInformation_DAO _applicationInformation;

        /// <summary>
        /// Primary Key. This is also a foreign key reference to the OfferInformation table.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, Column = "OfferInformationQuickCashDetailKey")]
        public virtual int Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// The Interest Rate applicable to the Quick Cash Payment
        /// </summary>
        [Property("InterestRate", ColumnType = "Double")]
        public virtual double? InterestRate
        {
            get
            {
                return this._interestRate;
            }
            set
            {
                this._interestRate = value;
            }
        }

        /// <summary>
        /// The Requested Amount for the particular Quick Cash Payment.
        /// </summary>
        [Property("RequestedAmount", ColumnType = "Double")]
        public virtual double? RequestedAmount
        {
            get
            {
                return this._requestedAmount;
            }
            set
            {
                this._requestedAmount = value;
            }
        }

        /// <summary>
        /// The Rate Configuration which applies to the Quick Cash Payment. This allows you to determine the margin and market
        /// rate for the Quick Cash Payment.
        /// </summary>
        [BelongsTo("RateConfigurationKey", NotNull = false)]
        public virtual RateConfiguration_DAO RateConfiguration
        {
            get
            {
                return this._rateConfiguration;
            }
            set
            {
                this._rateConfiguration = value;
            }
        }

        /// <summary>
        /// An indicator as to whether the Quick Cash payment has been disbursed.
        /// </summary>
        [Property("Disbursed", ColumnType = "Boolean")]
        public virtual bool? Disbursed
        {
            get
            {
                return this._disbursed;
            }
            set
            {
                this._disbursed = value;
            }
        }

        [BelongsTo("QuickCashPaymentTypeKey", NotNull = false)]
        public virtual QuickCashPaymentType_DAO QuickCashPaymentType
        {
            get
            {
                return this._quickCashPaymentType;
            }
            set
            {
                this._quickCashPaymentType = value;
            }
        }

        /// <summary>
        /// Each of the OfferInformationQuickCashDetail records belong to a single OfferInformationQuickCash key.
        /// </summary>
        [BelongsTo("OfferInformationKey", NotNull = true)]
        [ValidateNonEmpty("Application Information Quick Cash is a mandatory field")]
        public virtual ApplicationInformationQuickCash_DAO OfferInformationQuickCash
        {
            get
            {
                return this._offerInformationQuickCash;
            }
            set
            {
                this._offerInformationQuickCash = value;
            }
        }

        //[OneToOne]
        //public virtual ApplicationInformation_DAO ApplicationInformation
        //{
        //    get
        //    {
        //        return this._applicationInformation;
        //    }
        //    set
        //    {
        //        this._applicationInformation = value;
        //    }
        //}

        [HasAndBelongsToMany(typeof(ApplicationExpense_DAO), ColumnRef = "OfferExpenseKey", ColumnKey = "OfferInformationQuickCashDetailKey", Lazy = true, Schema = "dbo", Table = "OfferExpenseOfferInformationQuickCashDetail")]
        public virtual IList<ApplicationExpense_DAO> ApplicationExpenses
        {
            get
            {
                return this._applicationExpenses;
            }
            set
            {
                this._applicationExpenses = value;
            }
        }

        public virtual void Clone(ApplicationInformationQuickCashDetail_DAO QCD)
        {
            QCD.Disbursed = this.Disbursed;
            QCD.InterestRate = this.InterestRate;
            QCD.RateConfiguration = this.RateConfiguration;
            QCD.RequestedAmount = this.RequestedAmount;
        }
    }
}