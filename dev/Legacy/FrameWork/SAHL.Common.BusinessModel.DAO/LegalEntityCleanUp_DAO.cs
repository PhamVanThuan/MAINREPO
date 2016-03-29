using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("LegalEntityCleanUp", Schema = "dbo", Lazy = true)]
    public partial class LegalEntityCleanUp_DAO : DB_2AM<LegalEntityCleanUp_DAO>
    {
        private string _description;

        private string _surname;

        private string _firstnames;

        private string _iDNumber;

        private string _accounts;

        private int _Key;

        private LegalEntity_DAO _legalEntity;

        private LegalEntityExceptionReason_DAO _legalEntityExceptionReason;

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
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

        [Property("Surname", ColumnType = "String")]
        public virtual string Surname
        {
            get
            {
                return this._surname;
            }
            set
            {
                this._surname = value;
            }
        }

        [Property("Firstnames", ColumnType = "String")]
        public virtual string Firstnames
        {
            get
            {
                return this._firstnames;
            }
            set
            {
                this._firstnames = value;
            }
        }

        [Property("IDNumber", ColumnType = "String")]
        public virtual string IDNumber
        {
            get
            {
                return this._iDNumber;
            }
            set
            {
                this._iDNumber = value;
            }
        }

        [Property("Accounts", ColumnType = "String")]
        public virtual string Accounts
        {
            get
            {
                return this._accounts;
            }
            set
            {
                this._accounts = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "LegalEntityCleanUpKey", ColumnType = "Int32")]
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

        [BelongsTo("LegalEntityKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity is a mandatory field")]
        public virtual LegalEntity_DAO LegalEntity
        {
            get
            {
                return this._legalEntity;
            }
            set
            {
                this._legalEntity = value;
            }
        }

        [BelongsTo("LegalEntityExceptionReasonKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity Exception Reason is a mandatory field")]
        public virtual LegalEntityExceptionReason_DAO LegalEntityExceptionReason
        {
            get
            {
                return this._legalEntityExceptionReason;
            }
            set
            {
                this._legalEntityExceptionReason = value;
            }
        }
    }
}