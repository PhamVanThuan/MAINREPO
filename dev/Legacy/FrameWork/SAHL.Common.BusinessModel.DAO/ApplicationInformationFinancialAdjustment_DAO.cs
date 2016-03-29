using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    //[GenericTest(Globals.TestType.Find)]

    [ActiveRecord("OfferInformationFinancialAdjustment", Lazy = true, Schema = "dbo")]
    public partial class ApplicationInformationFinancialAdjustment_DAO : DB_2AM<ApplicationInformationFinancialAdjustment_DAO>
    {
        private int? _term;

        private Double? _fixedRate;

        private Double? _discount;

        private System.DateTime? _fromDate;

        private int _key;

        private ApplicationInformation_DAO _applicationInformation;

        private FinancialAdjustmentTypeSource_DAO _financialAdjustmentTypeSource;

        private IList<ApplicationInformationAppliedRateAdjustment_DAO> _applicationInformationAppliedRateAdjustments;

        /// <summary>
        /// The Term applicable to the Rate Override e.g. A CAP Rate Override has a term of 24 months.
        /// </summary>
        [Property("Term")] //, ColumnType="Int32")]
        public virtual int? Term
        {
            get
            {
                return this._term;
            }
            set
            {
                this._term = value;
            }
        }

        /// <summary>
        /// No rate override currently uses this property.
        /// </summary>
        [Property("FixedRate")] //, ColumnType="Double")]
        public virtual Double? FixedRate
        {
            get
            {
                return this._fixedRate;
            }
            set
            {
                this._fixedRate = value;
            }
        }

        /// <summary>
        /// Certain Rate Overrides, such as Super Lo, require a discount to be given to the client. This is the value of the
        /// discount.
        /// </summary>
        [Property("Discount")] //, ColumnType="Double")]
        public virtual Double? Discount
        {
            get
            {
                return this._discount;
            }
            set
            {
                this._discount = value;
            }
        }

        [Property("FromDate")]
        public virtual System.DateTime? FromDate
        {
            get
            {
                return this._fromDate;
            }
            set
            {
                this._fromDate = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>

        [PrimaryKey(PrimaryKeyType.Native, "OfferInformationFinancialAdjustmentKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        /// <summary>
        /// The ApplicationInformationRateOverride record belongs to an ApplicationInformation record.
        /// </summary>
        [BelongsTo("OfferInformationKey", NotNull = true)]
        [ValidateNonEmpty("Application Information is a mandatory field")]
        public virtual ApplicationInformation_DAO ApplicationInformation
        {
            get
            {
                return this._applicationInformation;
            }
            set
            {
                this._applicationInformation = value;
            }
        }

        [BelongsTo("FinancialAdjustmentTypeSourceKey", NotNull = true)]
        public virtual FinancialAdjustmentTypeSource_DAO FinancialAdjustmentTypeSource
        {
            get
            {
                return this._financialAdjustmentTypeSource;
            }
            set
            {
                this._financialAdjustmentTypeSource = value;
            }
        }

        [HasMany(typeof(ApplicationInformationAppliedRateAdjustment_DAO), Table = "OfferInformationAppliedRateAdjustment", ColumnKey = "OfferInformationFinancialAdjustmentKey", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ApplicationInformationAppliedRateAdjustment_DAO> ApplicationInformationAppliedRateAdjustments
        {
            get
            {
                if (_applicationInformationAppliedRateAdjustments == null)
                    _applicationInformationAppliedRateAdjustments = new List<ApplicationInformationAppliedRateAdjustment_DAO>();

                return this._applicationInformationAppliedRateAdjustments;
            }
            set
            {
                this._applicationInformationAppliedRateAdjustments = value;
            }
        }

        public virtual void Clone(ApplicationInformationFinancialAdjustment_DAO applicationInformationFinancialAdjustment)
        {
            applicationInformationFinancialAdjustment.Discount = this.Discount;
            applicationInformationFinancialAdjustment.FixedRate = this.FixedRate;
            applicationInformationFinancialAdjustment.FromDate = this.FromDate;
            applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource = this.FinancialAdjustmentTypeSource;
            applicationInformationFinancialAdjustment.Term = this.Term;

            foreach (ApplicationInformationAppliedRateAdjustment_DAO ra in this.ApplicationInformationAppliedRateAdjustments)
            {
                ApplicationInformationAppliedRateAdjustment_DAO newra = new ApplicationInformationAppliedRateAdjustment_DAO();
                newra.ADUser = ra.ADUser;
                newra.ApplicationElementValue = ra.ApplicationElementValue;
                newra.ApplicationInformationFinancialAdjustment = applicationInformationFinancialAdjustment;
                newra.ChangeDate = ra.ChangeDate;
                newra.RateAdjustmentElement = ra.RateAdjustmentElement;

                applicationInformationFinancialAdjustment.ApplicationInformationAppliedRateAdjustments.Add(newra);
            }
        }
    }
}