using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("FailedPropertyAddress", Schema = "mig")]
    public partial class FailedPropertyAddress_DAO : DB_2AM<FailedPropertyAddress_DAO>
    {
        private bool _isCleaned;

        private int _key;

        private Property_DAO _property;

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

        [PrimaryKey(PrimaryKeyType.Native, "FailedPropertyAddressKey", ColumnType = "Int32")]
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

        [BelongsTo("PropertyKey", NotNull = true)]
        [ValidateNonEmpty("Property is a mandatory field")]
        public virtual Property_DAO Property
        {
            get
            {
                return this._property;
            }
            set
            {
                this._property = value;
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