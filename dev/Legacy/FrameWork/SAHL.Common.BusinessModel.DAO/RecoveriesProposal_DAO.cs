using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("RecoveriesProposal", Schema = "recoveries", Lazy = false)]
    public partial class RecoveriesProposal_DAO : DB_2AM<RecoveriesProposal_DAO>
    {
        private int _key;

        private Account_DAO _account;

        private double _shortfallAmount;

        private double _repaymentAmount;

        private System.DateTime _startDate;

        private bool? _acknowledgementOfDebt;

        private ADUser_DAO _adUser;

        private System.DateTime _createDate;

        private GeneralStatus_DAO _generalStatus;

        [PrimaryKey(PrimaryKeyType.Native, "RecoveriesProposalKey", ColumnType = "Int32")]
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

        [Property("ShortfallAmount", ColumnType = "Double", NotNull = true)]
        public virtual double ShortfallAmount
        {
            get
            {
                return this._shortfallAmount;
            }
            set
            {
                this._shortfallAmount = value;
            }
        }

        [Property("RepaymentAmount", ColumnType = "Double", NotNull = true)]
        public virtual double RepaymentAmount
        {
            get
            {
                return this._repaymentAmount;
            }
            set
            {
                this._repaymentAmount = value;
            }
        }

        [Property("StartDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime StartDate
        {
            get
            {
                return this._startDate;
            }
            set
            {
                this._startDate = value;
            }
        }

        [Property("AcknowledgementOfDebt", ColumnType = "Boolean", NotNull = false)]
        public virtual bool? AcknowledgementOfDebt
        {
            get
            {
                return this._acknowledgementOfDebt;
            }
            set
            {
                this._acknowledgementOfDebt = value;
            }
        }

        [BelongsTo(Column = "ADUserKey", NotNull = true)]
        public virtual ADUser_DAO ADUser
        {
            get
            {
                return this._adUser;
            }
            set
            {
                this._adUser = value;
            }
        }

        [Property("CreateDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime CreateDate
        {
            get
            {
                return this._createDate;
            }
            set
            {
                this._createDate = value;
            }
        }

        [BelongsTo("GeneralStatusKey", NotNull = true)]
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
    }
}