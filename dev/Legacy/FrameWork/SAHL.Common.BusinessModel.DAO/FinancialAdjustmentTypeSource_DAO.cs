using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("FinancialAdjustmentTypeSource", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class FinancialAdjustmentTypeSource_DAO : DB_2AM<FinancialAdjustmentTypeSource_DAO>
    {
        private int _key;

        private FinancialAdjustmentSource_DAO _financialAdjustmentSource;

        private FinancialAdjustmentType_DAO _financialAdjustmentType;

        [PrimaryKey(PrimaryKeyType.Native, "FinancialAdjustmentTypeSourceKey", ColumnType = "Int32")]
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

        [BelongsTo("FinancialAdjustmentSourceKey")]
        [ValidateNonEmpty("Financial Adjustment Source is a mandatory field")]
        public virtual FinancialAdjustmentSource_DAO FinancialAdjustmentSource
        {
            get
            {
                return this._financialAdjustmentSource;
            }
            set
            {
                this._financialAdjustmentSource = value;
            }
        }

        [BelongsTo("FinancialAdjustmentTypeKey")]
        [ValidateNonEmpty("Financial Adjustment Type is a mandatory field")]
        public virtual FinancialAdjustmentType_DAO FinancialAdjustmentType
        {
            get
            {
                return this._financialAdjustmentType;
            }
            set
            {
                this._financialAdjustmentType = value;
            }
        }
    }
}