using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AccountBaselII", Schema = "dbo")]
    public partial class AccountBaselII_DAO : DB_2AM<AccountBaselII_DAO>
    {
        private int _key;

        private System.DateTime _accountingDate;

        private System.DateTime _processDate;

        private double _lGD;

        private double _eAD;

        private double _pD;

        private double _behaviouralScore;

        private int _eL;

        private Account_DAO _account;

        [PrimaryKey(PrimaryKeyType.Native, "AccountBaselIIKey", ColumnType = "Int32")]
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

        [Property("AccountingDate", ColumnType = "Timestamp")]
        public virtual System.DateTime AccountingDate
        {
            get
            {
                return this._accountingDate;
            }
            set
            {
                this._accountingDate = value;
            }
        }

        [Property("ProcessDate", ColumnType = "Timestamp")]
        public virtual System.DateTime ProcessDate
        {
            get
            {
                return this._processDate;
            }
            set
            {
                this._processDate = value;
            }
        }

        [Property("LGD", ColumnType = "Double")]
        public virtual double LGD
        {
            get
            {
                return this._lGD;
            }
            set
            {
                this._lGD = value;
            }
        }

        [Property("EAD", ColumnType = "Double")]
        public virtual double EAD
        {
            get
            {
                return this._eAD;
            }
            set
            {
                this._eAD = value;
            }
        }

        [Property("PD", ColumnType = "Double")]
        public virtual double PD
        {
            get
            {
                return this._pD;
            }
            set
            {
                this._pD = value;
            }
        }

        [Property("BehaviouralScore", ColumnType = "Double")]
        public virtual double BehaviouralScore
        {
            get
            {
                return this._behaviouralScore;
            }
            set
            {
                this._behaviouralScore = value;
            }
        }

        [Property("EL", ColumnType = "Int32")]
        public virtual int EL
        {
            get
            {
                return this._eL;
            }
            set
            {
                this._eL = value;
            }
        }

        [BelongsTo("AccountKey", NotNull = true)]
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
    }
}