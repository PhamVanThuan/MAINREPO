using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("BulkBatchParameter", Lazy = true, Schema = "dbo")]
    public partial class BulkBatchParameter_DAO : DB_2AM<BulkBatchParameter_DAO>
    {
        private string _parameterName;

        private string _parameterValue;

        private int _key;

        private BulkBatch_DAO _bulkBatch;

        [Property("ParameterName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Parameter Name is a mandatory field")]
        public virtual string ParameterName
        {
            get
            {
                return this._parameterName;
            }
            set
            {
                this._parameterName = value;
            }
        }

        [Property("ParameterValue", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Parameter Value is a mandatory field")]
        public virtual string ParameterValue
        {
            get
            {
                return this._parameterValue;
            }
            set
            {
                this._parameterValue = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "BulkBatchParameterKey", ColumnType = "Int32")]
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

        [BelongsTo("BulkBatchKey", NotNull = true)]
        [ValidateNonEmpty("Bulk Batch is a mandatory field")]
        public virtual BulkBatch_DAO BulkBatch
        {
            get
            {
                return this._bulkBatch;
            }
            set
            {
                this._bulkBatch = value;
            }
        }
    }
}