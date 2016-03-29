using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferInformationAppliedRateAdjustment", Lazy = true, Schema = "dbo")]
    public partial class ApplicationInformationAppliedRateAdjustment_DAO : DB_2AM<ApplicationInformationAppliedRateAdjustment_DAO>
    {
        private string _applicationElementValue;

        private System.DateTime _changeDate;

        private int _key;

        private ADUser_DAO _aDUser;

        private ApplicationInformationFinancialAdjustment_DAO _applicationInformationFinancialAdjustment;

        private RateAdjustmentElement_DAO _rateAdjustmentElement;

        [Property("OfferElementValue", ColumnType = "String")]
        public virtual string ApplicationElementValue
        {
            get
            {
                return this._applicationElementValue;
            }
            set
            {
                this._applicationElementValue = value;
            }
        }

        [Property("ChangeDate", NotNull = true)]
        public virtual System.DateTime ChangeDate
        {
            get
            {
                return this._changeDate;
            }
            set
            {
                this._changeDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OfferInformationAppliedRateAdjustmentKey", ColumnType = "Int32")]
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

        [BelongsTo("ADUserKey", NotNull = true)]
        [ValidateNonEmpty("ADUser is a mandatory field")]
        public virtual ADUser_DAO ADUser
        {
            get
            {
                return this._aDUser;
            }
            set
            {
                this._aDUser = value;
            }
        }

        [BelongsTo("OfferInformationFinancialAdjustmentKey", NotNull = true)]
        [ValidateNonEmpty("OfferInformationFinancialAdjustment is a mandatory field")]
        public virtual ApplicationInformationFinancialAdjustment_DAO ApplicationInformationFinancialAdjustment
        {
            get
            {
                return this._applicationInformationFinancialAdjustment;
            }
            set
            {
                this._applicationInformationFinancialAdjustment = value;
            }
        }

        [BelongsTo("RateAdjustmentElementKey", NotNull = true)]
        [ValidateNonEmpty("Rate Adjustment Element is a mandatory field")]
        public virtual RateAdjustmentElement_DAO RateAdjustmentElement
        {
            get
            {
                return this._rateAdjustmentElement;
            }
            set
            {
                this._rateAdjustmentElement = value;
            }
        }
    }
}