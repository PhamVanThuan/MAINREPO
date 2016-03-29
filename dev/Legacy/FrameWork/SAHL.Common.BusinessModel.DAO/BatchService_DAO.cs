using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("BatchService", Lazy = true, Schema = "dbo")]
    public partial class BatchService_DAO : DB_2AM<BatchService_DAO>
    {
        private int key;
        private System.DateTime requestedDate;
        private string requestedBy;
        private int batchCount;
        private int batchServiceTypeKey;
        private string fileName;
        private byte[] fileContent;

        [PrimaryKey(PrimaryKeyType.Native, "BatchServiceKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = value;
            }
        }


        [Property("RequestedBy", ColumnType = "String", NotNull = false)]
        public virtual string RequestedBy
        {
            get
            {
                return this.requestedBy;
            }
            set
            {
                this.requestedBy = value;
            }
        }


        [Property("RequestedDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime RequestedDate
        {
            get
            {
                return this.requestedDate;
            }
            set
            {
                this.requestedDate = value;
            }
        }


        [Property("BatchCount", ColumnType = "Int32", NotNull = true)]
        public virtual int BatchCount
        {
            get
            {
                return this.batchCount;
            }
            set
            {
                this.batchCount = value;
            }
        }


        [Property("BatchServiceTypeKey", ColumnType = "Int32", NotNull = true)]
        public virtual int BatchServiceTypeKey
        {
            get
            {
                return this.batchServiceTypeKey;
            }
            set
            {
                this.batchServiceTypeKey = value;
            }
        }

        [Property("FileName", ColumnType = "String")]
        public virtual string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
            }
        }

        [Property("FileContent", ColumnType = "Byte[]")]
        public virtual Byte[] FileContent
        {
            get
            {
                return this.fileContent;
            }
            set
            {
                this.fileContent = value;
            }
        }

    }
}
