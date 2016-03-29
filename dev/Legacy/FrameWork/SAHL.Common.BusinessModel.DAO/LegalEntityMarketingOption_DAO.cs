using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("LegalEntityMarketingOption", Schema = "dbo", Lazy = true)]
    public partial class LegalEntityMarketingOption_DAO : DB_2AM<LegalEntityMarketingOption_DAO>
    {
        private MarketingOption_DAO _marketingOption;

        private System.DateTime? _changeDate;

        private string _userID;

        private int _key;

        private LegalEntity_DAO _legalEntity;

        [BelongsTo("MarketingOptionKey", NotNull = true)]
        [ValidateNonEmpty("Marketing Option is a mandatory field")]
        public virtual MarketingOption_DAO MarketingOption
        {
            get
            {
                return this._marketingOption;
            }
            set
            {
                this._marketingOption = value;
            }
        }

        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = false)]
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

        [PrimaryKey(PrimaryKeyType.Native, "LegalEntityMarketingOptionKey", ColumnType = "Int32")]
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
    }
}