using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("TransactionTypeGroup", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class TransactionTypeGroup_DAO : DB_2AM<TransactionTypeGroup_DAO>
    {
        private int _key;

        private TransactionGroup_DAO _transactionGroup;

        private TransactionType_DAO _transactionType;

        [PrimaryKey(PrimaryKeyType.Native, "TransactionTypeGroupKey", ColumnType = "Int32")]
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

        [BelongsTo("TransactionGroupKey", NotNull = true)]
        [ValidateNonEmpty("Transaction Group is a mandatory field")]
        public virtual TransactionGroup_DAO TransactionGroup
        {
            get
            {
                return this._transactionGroup;
            }
            set
            {
                this._transactionGroup = value;
            }
        }

        [BelongsTo("TransactionTypeKey", NotNull = true)]
        [ValidateNonEmpty("Transaction Type is a mandatory field")]
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