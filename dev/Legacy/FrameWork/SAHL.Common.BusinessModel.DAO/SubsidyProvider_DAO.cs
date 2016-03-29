using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("SubsidyProvider", Schema = "dbo", Lazy = true)]
    public partial class SubsidyProvider_DAO : DB_2AM<SubsidyProvider_DAO>
    {
        private string _persalOrganisationCode;

        private string _contactPerson;

        private string _userID;

        private System.DateTime? _changeDate;

        private int _Key;

        private SubsidyProviderType_DAO _subsidyProviderType;

        private LegalEntity_DAO _legalEntity;

        private bool _gepfAffiliate;

        [Property("PersalOrganisationCode", ColumnType = "String")]
        public virtual string PersalOrganisationCode
        {
            get
            {
                return this._persalOrganisationCode;
            }
            set
            {
                this._persalOrganisationCode = value;
            }
        }

        [Property("ContactPerson", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Contact Person is a mandatory field")]
        public virtual string ContactPerson
        {
            get
            {
                return this._contactPerson;
            }
            set
            {
                this._contactPerson = value;
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

        [PrimaryKey(PrimaryKeyType.Native, "SubsidyProviderKey", ColumnType = "Int32")]
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

        [BelongsTo("SubsidyProviderTypeKey", NotNull = true)]
        [ValidateNonEmpty("Subsidy Provider Type is a mandatory field")]
        public virtual SubsidyProviderType_DAO SubsidyProviderType
        {
            get
            {
                return this._subsidyProviderType;
            }
            set
            {
                this._subsidyProviderType = value;
            }
        }

        [Property("GEPFAffiliate", ColumnType = "Boolean", NotNull = true)]
        public virtual bool GEPFAffiliate
        {
            get
            {
                return this._gepfAffiliate;
            }
            set
            {
                this._gepfAffiliate = value;
            }
        }
    }
}