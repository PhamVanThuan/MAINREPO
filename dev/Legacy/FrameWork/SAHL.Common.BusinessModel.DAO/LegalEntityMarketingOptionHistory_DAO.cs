using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    //[GenericTest(Globals.TestType.Find)]
    [ActiveRecord("LegalEntityMarketingOptionHistory", Schema = "dbo", Lazy = true)]
    public partial class LegalEntityMarketingOptionHistory_DAO : DB_2AM<LegalEntityMarketingOptionHistory_DAO>
    {
        private LegalEntityMarketingOption_DAO _legalEntityMarketingOption;

        private LegalEntity_DAO _legalEntity;

        private int _marketingOptionKey;

        private char _changeAction;

        private System.DateTime _changeDate;

        private string _userID;

        private int _key;

        [BelongsTo("LegalEntityMarketingOptionKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity Marketing Option is a mandatory field")]
        public virtual LegalEntityMarketingOption_DAO LegalEntityMarketingOptionKey
        {
            get
            {
                return this._legalEntityMarketingOption;
            }
            set
            {
                this._legalEntityMarketingOption = value;
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

        // TODO: Change to MarketingOption
        [Property("MarketingOptionKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Marketing Option is a mandatory field")]
        public virtual int MarketingOptionKey
        {
            get
            {
                return this._marketingOptionKey;
            }
            set
            {
                this._marketingOptionKey = value;
            }
        }

        [Property("ChangeAction", ColumnType = "Char", NotNull = true)]
        [ValidateNonEmpty("Change Action is a mandatory field")]
        public virtual Char ChangeAction
        {
            get
            {
                return this._changeAction;
            }
            set
            {
                this._changeAction = value;
            }
        }

        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = true, Length = 1)]
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

        [PrimaryKey(PrimaryKeyType.Native, "LegalEntityMarketingOptionHistoryKey", ColumnType = "Int32")]
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
    }
}