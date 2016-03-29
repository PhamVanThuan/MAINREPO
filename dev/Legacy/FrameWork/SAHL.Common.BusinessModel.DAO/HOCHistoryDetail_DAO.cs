using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("HOCHistoryDetail", Schema = "dbo")]
    public partial class HOCHistoryDetail_DAO : DB_2AM<HOCHistoryDetail_DAO>
    {
        private HOCHistory_DAO _hOCHistory;

        private System.DateTime _effectiveDate;

        private char _updateType;

        private double? _hOCThatchAmount;

        private double? _hOCConventionalAmount;

        private double? _hOCShingleAmount;

        private double? _hOCProrataPremium;

        private double? _hOCMonthlyPremium;

        private System.DateTime? _printDate;

        private System.DateTime? _sendFileDate;

        private System.DateTime _changeDate;

        private string _userID;

        private int _key;

        private double _hOCAdministrationFee;

        private double _hOCBasePremium;

        private double _sASRIAAmount;

        //[Property("HOCHistoryKey", ColumnType = "Int32", NotNull = true)]
        [BelongsTo("HOCHistoryKey", NotNull = true)]
        [ValidateNonEmpty("HOC History is a mandatory field")]
        public virtual HOCHistory_DAO HOCHistory
        {
            get
            {
                return this._hOCHistory;
            }
            set
            {
                this._hOCHistory = value;
            }
        }

        [Property("EffectiveDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Effective Date is a mandatory field")]
        public virtual System.DateTime EffectiveDate
        {
            get
            {
                return this._effectiveDate;
            }
            set
            {
                this._effectiveDate = value;
            }
        }

        [Property("UpdateType", ColumnType = "Char", NotNull = true, Length = 1)]
        [ValidateNonEmpty("Update Type is a mandatory field")]
        public virtual char UpdateType
        {
            get
            {
                return this._updateType;
            }
            set
            {
                this._updateType = value;
            }
        }

        [Property("HOCThatchAmount", ColumnType = "Double")]
        public virtual double? HOCThatchAmount
        {
            get
            {
                return this._hOCThatchAmount;
            }
            set
            {
                this._hOCThatchAmount = value;
            }
        }

        [Property("HOCConventionalAmount", ColumnType = "Double")]
        public virtual double? HOCConventionalAmount
        {
            get
            {
                return this._hOCConventionalAmount;
            }
            set
            {
                this._hOCConventionalAmount = value;
            }
        }

        [Property("HOCShingleAmount", ColumnType = "Double")]
        public virtual double? HOCShingleAmount
        {
            get
            {
                return this._hOCShingleAmount;
            }
            set
            {
                this._hOCShingleAmount = value;
            }
        }

        [Property("HOCProrataPremium", ColumnType = "Double")]
        public virtual double? HOCProrataPremium
        {
            get
            {
                return this._hOCProrataPremium;
            }
            set
            {
                this._hOCProrataPremium = value;
            }
        }

        [Property("HOCMonthlyPremium", ColumnType = "Double")]
        public virtual double? HOCMonthlyPremium
        {
            get
            {
                return this._hOCMonthlyPremium;
            }
            set
            {
                this._hOCMonthlyPremium = value;
            }
        }

        [Property("PrintDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? PrintDate
        {
            get
            {
                return this._printDate;
            }
            set
            {
                this._printDate = value;
            }
        }

        [Property("SendFileDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? SendFileDate
        {
            get
            {
                return this._sendFileDate;
            }
            set
            {
                this._sendFileDate = value;
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

        [PrimaryKey(PrimaryKeyType.Native, "HOCHistoryDetailKey", ColumnType = "Int32")]
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

        [Property("HOCAdministrationFee", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("HOC Administration Fee is a mandatory field")]
        public virtual double HOCAdministrationFee
        {
            get
            {
                return this._hOCAdministrationFee;
            }
            set
            {
                this._hOCAdministrationFee = value;
            }
        }

        [Property("HOCBasePremium", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("HOC Base Premium is a mandatory field")]
        public virtual double HOCBasePremium
        {
            get
            {
                return this._hOCBasePremium;
            }
            set
            {
                this._hOCBasePremium = value;
            }
        }

        [Property("SASRIAAmount", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("SASRIA Amount is a mandatory field")]
        public virtual double SASRIAAmount
        {
            get
            {
                return this._sASRIAAmount;
            }
            set
            {
                this._sASRIAAmount = value;
            }
        }
    }
}