using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Detail", Schema = "dbo", Lazy = true)]
    public partial class Detail_DAO : DB_2AM<Detail_DAO>
    {
        /// <summary>
        /// Nazir J - 20080725
        /// C# min date is much lower than the SQL min date (1753)
        /// Because persist is done even thou rules fail, Date has to set Date to higher
        /// than SQL min date
        /// </summary>
        private System.DateTime _detailDate = new System.DateTime(1800, 1, 1);

        private double? _amount;

        private string _description;

        private int? _linkID;

        private string _userID;

        private System.DateTime? _changeDate;

        private int _Key;

        private Account_DAO _account;

        private DetailType_DAO _detailType;

        [Lurker]
        [Property("DetailDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Detail Date is a mandatory field")]
        public virtual System.DateTime DetailDate
        {
            get
            {
                return this._detailDate;
            }
            set
            {
                this._detailDate = value;
            }
        }

        [Lurker]
        [Property("Amount", ColumnType = "Double")]
        public virtual double? Amount
        {
            get
            {
                return this._amount;
            }
            set
            {
                this._amount = value;
            }
        }

        [Lurker]
        [Property("Description", ColumnType = "String")]
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

        [Lurker]
        [Property("LinkID", ColumnType = "Int32")]
        public virtual int? LinkID
        {
            get
            {
                return this._linkID;
            }
            set
            {
                this._linkID = value;
            }
        }

        [Lurker]
        [Property("UserID", ColumnType = "String")]
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

        [Lurker]
        [Property("ChangeDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ChangeDate
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

        [PrimaryKey(PrimaryKeyType.Native, "DetailKey", ColumnType = "Int32")]
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

        [Lurker]
        [BelongsTo("AccountKey", NotNull = true)]
        [ValidateNonEmpty("Account is a mandatory field")]
        public virtual Account_DAO Account
        {
            get
            {
                return this._account;
            }
            set
            {
                this._account = value;
            }
        }

        [Lurker]
        [BelongsTo("DetailTypeKey", NotNull = true)]
        [ValidateNonEmpty("Detail Type is a mandatory field")]
        public virtual DetailType_DAO DetailType
        {
            get
            {
                return this._detailType;
            }
            set
            {
                this._detailType = value;
            }
        }
    }
}