using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("TransactionTypeDataAccess", Schema = "dbo")]
    public partial class TransactionTypeDataAccess_DAO : DB_2AM<TransactionTypeDataAccess_DAO>
    {
        private string _aDCredentials;

        private int _transactionTypeKey;

        private int _Key;

        //private IList<TransactionType_DAO> _transactionType;

        [Property("ADCredentials", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("AD Credentials is a mandatory field")]
        public virtual string ADCredentials
        {
            get
            {
                return this._aDCredentials;
            }
            set
            {
                this._aDCredentials = value;
            }
        }

        [Property("TransactionTypeKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Transaction Type Number is a mandatory field")]
        public virtual int TransactionTypeKey
        {
            get
            {
                return this._transactionTypeKey;
            }
            set
            {
                this._transactionTypeKey = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "TransactionTypeDataAccessKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(TransactionType_DAO), ColumnKey = "TransactionTypeKey", Table = "TransactionType")]
        //private IList<TransactionType_DAO> TransactionType
        //{
        //    get
        //    {
        //        return this._transactionType;
        //    }
        //    set
        //    {
        //        this._transactionType = value;
        //    }
        //}
    }
}