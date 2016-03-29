using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("FeeType", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class FeeType_DAO : DB_2AM<FeeType_DAO>
    {
        private int _key;

        private string _description;

        private GeneralStatus_DAO _generalStatus;

        private FeeFrequency_DAO _feeFrequency;

        private TransactionType_DAO _transactionType;

        [PrimaryKey(PrimaryKeyType.Assigned, "FeeTypeKey", ColumnType = "Int32")]
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

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
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

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
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

        [BelongsTo("FeeFrequencyKey")]
        public virtual FeeFrequency_DAO FeeFrequency
        {
            get
            {
                return this._feeFrequency;
            }
            set
            {
                this._feeFrequency = value;
            }
        }

        [BelongsTo("TransactionTypeKey")]
        public virtual TransactionType_DAO TransactionType
        {
            get
            {
                return this._transactionType;
            }
            set
            {
                this._transactionType = value;
            }
        }
    }
}