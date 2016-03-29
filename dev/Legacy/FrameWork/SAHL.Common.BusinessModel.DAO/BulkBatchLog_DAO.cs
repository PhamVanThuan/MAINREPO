using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("BulkBatchLog", Lazy = true, Schema = "dbo")]
    public partial class BulkBatchLog_DAO : DB_2AM<BulkBatchLog_DAO>
    {
        private string _description;

        private string _messageReference;

        private string _messageReferenceKey;

        private int _key;

        private BulkBatch_DAO _bulkBatch;

        private MessageType_DAO _messageType;

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

        [Property("MessageReference", ColumnType = "String")]
        public virtual string MessageReference
        {
            get
            {
                return this._messageReference;
            }
            set
            {
                this._messageReference = value;
            }
        }

        [Property("MessageReferenceKey", ColumnType = "String")]
        public virtual string MessageReferenceKey
        {
            get
            {
                return this._messageReferenceKey;
            }
            set
            {
                this._messageReferenceKey = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "BulkBatchLogKey", ColumnType = "Int32")]
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

        [BelongsTo("MessageTypeKey", NotNull = true)]
        [ValidateNonEmpty("Message Type is a mandatory field")]
        public virtual MessageType_DAO MessageType
        {
            get
            {
                return this._messageType;
            }
            set
            {
                this._messageType = value;
            }
        }
    }
}