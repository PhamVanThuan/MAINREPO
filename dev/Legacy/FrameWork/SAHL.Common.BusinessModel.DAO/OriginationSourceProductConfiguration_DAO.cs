using Castle.ActiveRecord;

using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OriginationSourceProductConfiguration", Lazy = true, Schema = "dbo")]
    public partial class OriginationSourceProductConfiguration_DAO : DB_2AM<OriginationSourceProductConfiguration_DAO>
    {
        private int _Key;

        private FinancialServiceType_DAO _financialServiceType;

        private MarketRate_DAO _marketRate;

        private OriginationSourceProduct_DAO _originationSourceProduct;

        private ResetConfiguration_DAO _resetConfiguration;

        [PrimaryKey(PrimaryKeyType.Native, "OSPConfigurationKey", ColumnType = "Int32")]
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

        [BelongsTo("MarketRateKey", NotNull = false)]
        public virtual MarketRate_DAO MarketRate
        {
            get
            {
                return this._marketRate;
            }
            set
            {
                this._marketRate = value;
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

        [BelongsTo("ResetConfigurationKey", NotNull = false)]
        public virtual ResetConfiguration_DAO ResetConfiguration
        {
            get
            {
                return this._resetConfiguration;
            }
            set
            {
                this._resetConfiguration = value;
            }
        }
    }
}