using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ProductCondition", Schema = "dbo")]
    public partial class ProductCondition_DAO : DB_2AM<ProductCondition_DAO>
    {
        private int _purposeKey;

        private string _applicationName;

        private int _Key;

        private Condition_DAO _condition;

        private FinancialServiceType_DAO _financialServiceType;

        private OriginationSourceProduct_DAO _originationSourceProduct;

        private ProductConditionType_DAO _productConditionType;

        [Property("PurposeKey", ColumnType = "Int32")]
        public virtual int PurposeKey
        {
            get
            {
                return this._purposeKey;
            }
            set
            {
                this._purposeKey = value;
            }
        }

        [Property("ApplicationName", ColumnType = "String")]
        public virtual string ApplicationName
        {
            get
            {
                return this._applicationName;
            }
            set
            {
                this._applicationName = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ProductConditionKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        [BelongsTo("ConditionKey", NotNull = false)]
        public virtual Condition_DAO Condition
        {
            get
            {
                return this._condition;
            }
            set
            {
                this._condition = value;
            }
        }

        [BelongsTo("FinancialServiceTypeKey", NotNull = false)]
        public virtual FinancialServiceType_DAO FinancialServiceType
        {
            get
            {
                return this._financialServiceType;
            }
            set
            {
                this._financialServiceType = value;
            }
        }

        [BelongsTo("OriginationSourceProductKey", NotNull = false)]
        public virtual OriginationSourceProduct_DAO OriginationSourceProduct
        {
            get
            {
                return this._originationSourceProduct;
            }
            set
            {
                this._originationSourceProduct = value;
            }
        }

        [BelongsTo("ProductConditionTypeKey", NotNull = false)]
        public virtual ProductConditionType_DAO ProductConditionType
        {
            get
            {
                return this._productConditionType;
            }
            set
            {
                this._productConditionType = value;
            }
        }
    }
}