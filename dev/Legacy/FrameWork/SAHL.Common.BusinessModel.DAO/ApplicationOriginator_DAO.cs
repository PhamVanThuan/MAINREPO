using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferOriginator", Schema = "dbo", Lazy = true)]
    public partial class ApplicationOriginator_DAO : DB_2AM<ApplicationOriginator_DAO>
    {
        private string _contact;

        private int _key;

        private GeneralStatus_DAO _generalStatus;

        private LegalEntity_DAO _legalEntity;

        private OriginationSource_DAO _originationSource;

        [Property("Contact", ColumnType = "String")]
        public virtual string Contact
        {
            get
            {
                return this._contact;
            }
            set
            {
                this._contact = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OfferOriginatorKey", ColumnType = "Int32")]
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

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
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

        [BelongsTo("OriginationSourceKey", NotNull = true)]
        [ValidateNonEmpty("Origination Source is a mandatory field")]
        public virtual OriginationSource_DAO OriginationSource
        {
            get
            {
                return this._originationSource;
            }
            set
            {
                this._originationSource = value;
            }
        }
    }
}