using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OSPFinancialAdjustmentTypeSource", Schema = "dbo")]
    public partial class OSPFinancialAdjustmentTypeSource_DAO : DB_2AM<OSPFinancialAdjustmentTypeSource_DAO>
    {
        private int _key;

        private FinancialAdjustmentTypeSource_DAO _financialAdjustmentTypeSource;

        private OriginationSourceProduct_DAO _originationSourceProduct;

        [PrimaryKey(PrimaryKeyType.Native, "OSPFinancialAdjustmentTypeSourceKey", ColumnType = "Int32")]
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

        [BelongsTo("OriginationSourceProductKey", NotNull = true)]
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
    }
}