using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("CDV", Schema = "dbo")]
    public partial class CDV_DAO : DB_2AM<CDV_DAO>
    {
        private int _aCBTypeNumber;

        private int? _streamCode;

        private int? _exceptionStreamCode;

        private string _weightings;

        private int? _modulus;

        private int? _fudgeFactor;

        private string _exceptionCode;

        private int? _accountIndicator;

        private string _userID;

        private System.DateTime _dateChange;

        private int _Key;

        private ACBBank_DAO _aCBBank;

        private ACBBranch_DAO _aCBBranch;

        [Property("ACBTypeNumber", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("ACB Type Number is a mandatory field")]
        public virtual int ACBTypeNumber
        {
            get
            {
                return this._aCBTypeNumber;
            }
            set
            {
                this._aCBTypeNumber = value;
            }
        }

        [Property("StreamCode", ColumnType = "Int32")]
        public virtual int? StreamCode
        {
            get
            {
                return this._streamCode;
            }
            set
            {
                this._streamCode = value;
            }
        }

        [Property("ExceptionStreamCode", ColumnType = "Int32")]
        public virtual int? ExceptionStreamCode
        {
            get
            {
                return this._exceptionStreamCode;
            }
            set
            {
                this._exceptionStreamCode = value;
            }
        }

        [Property("Weightings", ColumnType = "String")]
        public virtual string Weightings
        {
            get
            {
                return this._weightings;
            }
            set
            {
                this._weightings = value;
            }
        }

        [Property("Modulus", ColumnType = "Int32")]
        public virtual int? Modulus
        {
            get
            {
                return this._modulus;
            }
            set
            {
                this._modulus = value;
            }
        }

        [Property("FudgeFactor", ColumnType = "Int32")]
        public virtual int? FudgeFactor
        {
            get
            {
                return this._fudgeFactor;
            }
            set
            {
                this._fudgeFactor = value;
            }
        }

        [Property("ExceptionCode", ColumnType = "String", Length = 2)]
        public virtual string ExceptionCode
        {
            get
            {
                return this._exceptionCode;
            }
            set
            {
                this._exceptionCode = value;
            }
        }

        [Property("AccountIndicator", ColumnType = "Int32")]
        public virtual int? AccountIndicator
        {
            get
            {
                return this._accountIndicator;
            }
            set
            {
                this._accountIndicator = value;
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

        [Property("DateChange", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("DateChange is a mandatory field")]
        public virtual System.DateTime DateChange
        {
            get
            {
                return this._dateChange;
            }
            set
            {
                this._dateChange = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "CDVKey", ColumnType = "Int32")]
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

        [BelongsTo("ACBBankCode", NotNull = true)]
        [ValidateNonEmpty("ACB Bank is a mandatory field")]
        public virtual ACBBank_DAO ACBBank
        {
            get
            {
                return this._aCBBank;
            }
            set
            {
                this._aCBBank = value;
            }
        }

        [BelongsTo("ACBBranchCode", NotNull = true)]
        [ValidateNonEmpty("ACB Branch is a mandatory field")]
        public virtual ACBBranch_DAO ACBBranch
        {
            get
            {
                return this._aCBBranch;
            }
            set
            {
                this._aCBBranch = value;
            }
        }
    }
}