using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("CDCIM900Exceptions", Schema = "dbo")]
    public partial class CDCIM900Exceptions_DAO : DB_2AM<CDCIM900Exceptions_DAO>
    {
        private System.DateTime _fileDate;

        private int _recordNumber;

        private int _branchNumber;

        private string _exception;

        private System.DateTime _actionDate;

        private int _Key;

        [Property("FileDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("File Date is a mandatory field")]
        public virtual System.DateTime FileDate
        {
            get
            {
                return this._fileDate;
            }
            set
            {
                this._fileDate = value;
            }
        }

        [Property("RecordNumber", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Record Number is a mandatory field")]
        public virtual int RecordNumber
        {
            get
            {
                return this._recordNumber;
            }
            set
            {
                this._recordNumber = value;
            }
        }

        [Property("BranchNumber", ColumnType = "Int32")]
        public virtual int BranchNumber
        {
            get
            {
                return this._branchNumber;
            }
            set
            {
                this._branchNumber = value;
            }
        }

        [Property("Exception", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Exception is a mandatory field")]
        public virtual string Exception
        {
            get
            {
                return this._exception;
            }
            set
            {
                this._exception = value;
            }
        }

        [Property("ActionDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Action Date is a mandatory field")]
        public virtual System.DateTime ActionDate
        {
            get
            {
                return this._actionDate;
            }
            set
            {
                this._actionDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "CDCIM900ExceptionsKey", ColumnType = "Int32")]
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
    }
}