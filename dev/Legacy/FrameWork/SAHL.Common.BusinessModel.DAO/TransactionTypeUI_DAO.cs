using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("TransactionTypeUI", Schema = "dbo", Lazy = true)]
    public class TransactionTypeUI_DAO : DB_2AM<TransactionTypeUI_DAO>
    {
        private int _key;

        private TransactionType_DAO _transactionType;

        private int _screenBatch;

        private string _htmlColour;

        private bool _memo;

        [PrimaryKey(PrimaryKeyType.Native, "TransactionTypeUIKey", ColumnType = "Int32")]
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

        [BelongsTo("TransactionTypeKey", NotNull = true)]
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

        [Property("ScreenBatch", ColumnType = "Int32", NotNull = true)]
        public virtual int ScreenBatch
        {
            get
            {
                return this._screenBatch;
            }
            set
            {
                this._screenBatch = value;
            }
        }

        [Property("HTMLColour", ColumnType = "String")]
        public virtual string HTMLColour
        {
            get
            {
                return this._htmlColour;
            }
            set
            {
                this._htmlColour = value;
            }
        }

        [Property("Memo", ColumnType = "Boolean", NotNull = true)]
        public virtual bool Memo
        {
            get
            {
                return this._memo;
            }
            set
            {
                this._memo = value;
            }
        }
    }
}