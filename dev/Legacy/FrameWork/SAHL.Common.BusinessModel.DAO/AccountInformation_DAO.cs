using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AccountInformation", Schema = "dbo", Lazy = true)]
    public partial class AccountInformation_DAO : DB_2AM<AccountInformation_DAO>
    {
        private System.DateTime? _entryDate;

        private double? _amount;

        private string _information;

        private int _Key;

        private Account_DAO _account;

        private AccountInformationType_DAO _accountInformationType;

        [Property("EntryDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? EntryDate
        {
            get
            {
                return this._entryDate;
            }
            set
            {
                this._entryDate = value;
            }
        }

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

        [Property("Information", ColumnType = "String")]
        public virtual string Information
        {
            get
            {
                return this._information;
            }
            set
            {
                this._information = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "AccountInformationKey", ColumnType = "Int32")]
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

        [BelongsTo("AccountKey", NotNull = false)]
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

        [BelongsTo("AccountInformationTypeKey", NotNull = false)]
        public virtual AccountInformationType_DAO AccountInformationType
        {
            get
            {
                return this._accountInformationType;
            }
            set
            {
                this._accountInformationType = value;
            }
        }
    }
}