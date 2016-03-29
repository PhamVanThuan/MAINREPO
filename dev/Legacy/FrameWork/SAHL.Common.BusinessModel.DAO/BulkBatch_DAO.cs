using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("BulkBatch", Lazy = true, Schema = "dbo")]
    public partial class BulkBatch_DAO : DB_2AM<BulkBatch_DAO>
    {
        private string _description;

        private int _identifierReferenceKey;

        private System.DateTime _effectiveDate = new DateTime(1900, 1, 1); // initialise to 1900 to prevent SQL Server errors

        private System.DateTime? _startDateTime;

        private System.DateTime? _completedDateTime;

        private string _fileName;

        private string _userID;

        //private System.DateTime _changeDate;

        private int _key;

        private IList<BatchTransaction_DAO> _batchTransactions;

        private IList<BulkBatchLog_DAO> _bulkBatchLogs;

        private IList<BulkBatchParameter_DAO> _bulkBatchParameters;

        private BulkBatchStatus_DAO _bulkBatchStatus;

        private BulkBatchType_DAO _bulkBatchType;

        [Property("Description", ColumnType = "String")]
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

        [Property("IdentifierReferenceKey", ColumnType = "Int32")]
        public virtual int IdentifierReferenceKey
        {
            get
            {
                return this._identifierReferenceKey;
            }
            set
            {
                this._identifierReferenceKey = value;
            }
        }

        [Property("EffectiveDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Effective Date is a mandatory field")]
        public virtual System.DateTime EffectiveDate
        {
            get
            {
                return this._effectiveDate;
            }
            set
            {
                this._effectiveDate = value;
            }
        }

        [Property("StartDateTime")]
        public virtual System.DateTime? StartDateTime
        {
            get
            {
                return this._startDateTime;
            }
            set
            {
                this._startDateTime = value;
            }
        }

        [Property("CompletedDateTime")]
        public virtual System.DateTime? CompletedDateTime
        {
            get
            {
                return this._completedDateTime;
            }
            set
            {
                this._completedDateTime = value;
            }
        }

        [Property("FileName", ColumnType = "String")]
        public virtual string FileName
        {
            get
            {
                return this._fileName;
            }
            set
            {
                this._fileName = value;
            }
        }

        [Property("UserID", ColumnType = "String", NotNull = false)]
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

        //[Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
        //public virtual System.DateTime ChangeDate
        //{
        //    get
        //    {
        //        return this._changeDate;
        //    }
        //    set
        //    {
        //        this._changeDate = value;
        //    }
        //}

        [PrimaryKey(PrimaryKeyType.Native, "BulkBatchKey", ColumnType = "Int32")]
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

        [HasMany(typeof(BatchTransaction_DAO), ColumnKey = "BulkBatchKey", Table = "BatchTransaction", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<BatchTransaction_DAO> BatchTransactions
        {
            get
            {
                return this._batchTransactions;
            }
            set
            {
                this._batchTransactions = value;
            }
        }

        [HasMany(typeof(BulkBatchLog_DAO), ColumnKey = "BulkBatchKey", Table = "BulkBatchLog", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<BulkBatchLog_DAO> BulkBatchLogs
        {
            get
            {
                return this._bulkBatchLogs;
            }
            set
            {
                this._bulkBatchLogs = value;
            }
        }

        [HasMany(typeof(BulkBatchParameter_DAO), ColumnKey = "BulkBatchKey", Table = "BulkBatchParameter", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<BulkBatchParameter_DAO> BulkBatchParameters
        {
            get
            {
                return this._bulkBatchParameters;
            }
            set
            {
                this._bulkBatchParameters = value;
            }
        }

        [BelongsTo("BulkBatchStatusKey", NotNull = true)]
        [ValidateNonEmpty("Bulk Batch Status is a mandatory field")]
        public virtual BulkBatchStatus_DAO BulkBatchStatus
        {
            get
            {
                return this._bulkBatchStatus;
            }
            set
            {
                this._bulkBatchStatus = value;
            }
        }

        [BelongsTo("BulkBatchTypeKey", NotNull = true)]
        [ValidateNonEmpty("Bulk Batch Type is a mandatory field")]
        public virtual BulkBatchType_DAO BulkBatchType
        {
            get
            {
                return this._bulkBatchType;
            }
            set
            {
                this._bulkBatchType = value;
            }
        }
    }
}