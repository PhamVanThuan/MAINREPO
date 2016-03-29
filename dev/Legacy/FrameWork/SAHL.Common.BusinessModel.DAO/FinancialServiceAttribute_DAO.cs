using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("FinancialServiceAttribute", Schema = "product", Lazy = true)]
    public partial class FinancialServiceAttribute_DAO : DB_2AM<FinancialServiceAttribute_DAO>
    {
        private int _key;

        private FinancialService_DAO _financialService;

        private FinancialServiceAttributeType_DAO _financialServiceAttributeType;

        private IList<FinancialAdjustment_DAO> _financialAdjustments;

        private GeneralStatus_DAO _generalStatus;

        [PrimaryKey(PrimaryKeyType.Native, "FinancialServiceAttributeKey", ColumnType = "Int32")]
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

        [BelongsTo("FinancialServiceKey", NotNull = true, Lazy = FetchWhen.OnInvoke)]
        public virtual FinancialService_DAO FinancialService
        {
            get
            {
                return this._financialService;
            }
            set
            {
                this._financialService = value;
            }
        }

        [BelongsTo("FinancialServiceAttributeTypeKey", NotNull = true)]
        public virtual FinancialServiceAttributeType_DAO FinancialServiceAttributeType
        {
            get
            {
                return this._financialServiceAttributeType;
            }
            set
            {
                this._financialServiceAttributeType = value;
            }
        }

        [HasMany(typeof(FinancialAdjustment_DAO), ColumnKey = "FinancialServiceAttributeKey", Table = "FinancialAdjustment", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<FinancialAdjustment_DAO> FinancialAdjustments
        {
            get
            {
                return this._financialAdjustments;
            }
            set
            {
                this._financialAdjustments = value;
            }
        }
    }
}