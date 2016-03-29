using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("CDVExceptions", Schema = "dbo")]
    public partial class CDVExceptions_DAO : DB_2AM<CDVExceptions_DAO>
    {
        private int? _noOfDigits;

        private string _weightings;

        private int? _modulus;

        private int? _fudgeFactor;

        private string _exceptionCode;

        private string _userID;

        private System.DateTime _dateChange;

        private int _Key;

        private ACBBank_DAO _aCBBank;

        private ACBType_DAO _aCBType;

        [Property("NoOfDigits", ColumnType = "Int32")]
        public virtual int? NoOfDigits
        {
            get
            {
                return this._noOfDigits;
            }
            set
            {
                this._noOfDigits = value;
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

        [Property("ExceptionCode", ColumnType = "String", NotNull = true, Length = 2)]
        [ValidateNonEmpty("Exception Code is a mandatory field")]
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
        [ValidateNonEmpty("Date Change is a mandatory field")]
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

        [PrimaryKey(PrimaryKeyType.Native, "CDVExceptionsKey", ColumnType = "Int32")]
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

        [BelongsTo("ACBTypeNumber", NotNull = true)]
        [ValidateNonEmpty("ACB Type is a mandatory field")]
        public virtual ACBType_DAO ACBType
        {
            get
            {
                return this._aCBType;
            }
            set
            {
                this._aCBType = value;
            }
        }
    }
}