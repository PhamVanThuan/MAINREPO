using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("FailedLegalEntityAddress", Schema = "mig", Lazy = true)]
    public partial class FailedLegalEntityAddress_DAO : DB_2AM<FailedLegalEntityAddress_DAO>
    {
        private bool _isCleaned;

        private bool _postalIsCleaned;

        private int _key;

        private LegalEntity_DAO _legalEntity;

        private FailedPostalMigration_DAO _failedPostalMigration;

        private FailedStreetMigration_DAO _failedStreetMigration;

        [Property("IsCleaned", ColumnType = "Boolean")]
        public virtual bool IsCleaned
        {
            get
            {
                return this._isCleaned;
            }
            set
            {
                this._isCleaned = value;
            }
        }

        [Property("PostalIsCleaned", ColumnType = "Boolean")]
        public virtual bool PostalIsCleaned
        {
            get
            {
                return this._postalIsCleaned;
            }
            set
            {
                this._postalIsCleaned = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "FailedLegalEntityAddressKey", ColumnType = "Int32")]
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

        [BelongsTo("FailedPostalMigrationKey")]
        public virtual FailedPostalMigration_DAO FailedPostalMigration
        {
            get
            {
                return this._failedPostalMigration;
            }
            set
            {
                this._failedPostalMigration = value;
            }
        }

        [BelongsTo("FailedStreetMigrationKey")]
        public virtual FailedStreetMigration_DAO FailedStreetMigration
        {
            get
            {
                return this._failedStreetMigration;
            }
            set
            {
                this._failedStreetMigration = value;
            }
        }
    }
}