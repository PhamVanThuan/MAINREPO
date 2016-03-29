using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("RateAdjustmentElement", Schema = "dbo")]
    public partial class RateAdjustmentElement_DAO : DB_2AM<RateAdjustmentElement_DAO>
    {
        private double? _elementMinValue;

        private double? _elementMaxValue;

        private string _elementText;

        private double _rateAdjustmentValue;

        private System.DateTime _effectiveDate;

        private string _description;

        private int _key;

        private GeneralStatus_DAO _generalStatus;

        private GenericKeyType_DAO _genericKeyType;

        private RateAdjustmentElementType_DAO _rateAdjustmentElementType;

        private RateAdjustmentGroup_DAO _rateAdjustmentGroup;

        private FinancialAdjustmentTypeSource_DAO _financialAdjustmentTypeSource;

        private RuleItem_DAO _ruleItem;

        [Property("ElementMinValue")]
        public virtual double? ElementMinValue
        {
            get
            {
                return this._elementMinValue;
            }
            set
            {
                this._elementMinValue = value;
            }
        }

        [Property("ElementMaxValue")]
        public virtual double? ElementMaxValue
        {
            get
            {
                return this._elementMaxValue;
            }
            set
            {
                this._elementMaxValue = value;
            }
        }

        [Property("ElementText", ColumnType = "String")]
        public virtual string ElementText
        {
            get
            {
                return this._elementText;
            }
            set
            {
                this._elementText = value;
            }
        }

        [Property("RateAdjustmentValue", NotNull = true)]
        public virtual double RateAdjustmentValue
        {
            get
            {
                return this._rateAdjustmentValue;
            }
            set
            {
                this._rateAdjustmentValue = value;
            }
        }

        [Property("EffectiveDate", NotNull = true)]
        public virtual System.DateTime EffectiveDate
        {
            get
            {
                return this._effectiveDate;
            }
            set
            {
                this._effectiveDate = value;
            }
        }

        [Property("Description", ColumnType = "String", NotNull = true)]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "RateAdjustmentElementKey", ColumnType = "Int32")]
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

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        [BelongsTo("GenericKeyTypeKey", NotNull = true)]
        public virtual GenericKeyType_DAO GenericKeyType
        {
            get
            {
                return this._genericKeyType;
            }
            set
            {
                this._genericKeyType = value;
            }
        }

        [BelongsTo("RateAdjustmentElementTypeKey", NotNull = true)]
        public virtual RateAdjustmentElementType_DAO RateAdjustmentElementType
        {
            get
            {
                return this._rateAdjustmentElementType;
            }
            set
            {
                this._rateAdjustmentElementType = value;
            }
        }

        [BelongsTo("RateAdjustmentGroupKey", NotNull = true)]
        public virtual RateAdjustmentGroup_DAO RateAdjustmentGroup
        {
            get
            {
                return this._rateAdjustmentGroup;
            }
            set
            {
                this._rateAdjustmentGroup = value;
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

        [BelongsTo("RuleItemKey", NotNull = true)]
        public virtual RuleItem_DAO RuleItem
        {
            get
            {
                return this._ruleItem;
            }
            set
            {
                this._ruleItem = value;
            }
        }
    }
}