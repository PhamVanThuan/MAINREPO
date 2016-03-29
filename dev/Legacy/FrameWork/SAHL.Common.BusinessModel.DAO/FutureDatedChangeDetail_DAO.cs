using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("FutureDatedChangeDetail", Schema = "dbo")]
    public partial class FutureDatedChangeDetail_DAO : DB_2AM<FutureDatedChangeDetail_DAO>
    {
        private int _referenceKey;

        private char _action;

        private string _tableName;

        private string _columnName;

        private string _value;

        private string _userID;

        private System.DateTime _changeDate;

        private int _Key;

        private FutureDatedChange_DAO _futureDatedChange;

        [Property("ReferenceKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Reference Key is a mandatory field")]
        public virtual int ReferenceKey
        {
            get
            {
                return this._referenceKey;
            }
            set
            {
                this._referenceKey = value;
            }
        }

        [Property("Action", ColumnType = "AnsiChar", NotNull = true)]
        [ValidateNonEmpty("Action is a mandatory field")]
        public virtual char Action
        {
            get
            {
                return this._action;
            }
            set
            {
                this._action = value;
            }
        }

        [Property("TableName", ColumnType = "String", NotNull = true, Length = 255)]
        [ValidateNonEmpty("Table Name is a mandatory field")]
        public virtual string TableName
        {
            get
            {
                return this._tableName;
            }
            set
            {
                this._tableName = value;
            }
        }

        [Property("ColumnName", ColumnType = "String", Length = 255)]
        public virtual string ColumnName
        {
            get
            {
                return this._columnName;
            }
            set
            {
                this._columnName = value;
            }
        }

        [Property("Value", ColumnType = "String", NotNull = true, Length = 255)]
        [ValidateNonEmpty("Value is a mandatory field")]
        public virtual string Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        [Property("UserID", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("User ID is a mandatory field")]
        public virtual string UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
            }
        }

        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Change Date is a mandatory field")]
        public virtual System.DateTime ChangeDate
        {
            get
            {
                return this._changeDate;
            }
            set
            {
                this._changeDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "FutureDatedChangeDetailKey", ColumnType = "Int32")]
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

        [BelongsTo("FutureDatedChangeKey", NotNull = true)]
        [ValidateNonEmpty("Future Dated Change is a mandatory field")]
        public virtual FutureDatedChange_DAO FutureDatedChange
        {
            get
            {
                return this._futureDatedChange;
            }
            set
            {
                this._futureDatedChange = value;
            }
        }
    }
}