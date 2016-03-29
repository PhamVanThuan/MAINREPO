using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ImportFile", Schema = "dbo")]
    public partial class ImportFile_DAO : DB_2AM<ImportFile_DAO>
    {
        private string _fileName;

        private string _fileType;

        private System.DateTime _dateImported;

        private string _status;

        private string _userID;

        private string _xmlData;

        private int _key;

        [Property("FileName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("File Name is a mandatory field")]
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

        [Property("FileType", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("File Type is a mandatory field")]
        public virtual string FileType
        {
            get
            {
                return this._fileType;
            }
            set
            {
                this._fileType = value;
            }
        }

        [Property("DateImported", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Date Imported is a mandatory field")]
        public virtual System.DateTime DateImported
        {
            get
            {
                return this._dateImported;
            }
            set
            {
                this._dateImported = value;
            }
        }

        [Property("Status", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Status is a mandatory field")]
        public virtual string Status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
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

        [Property("XML_Data", ColumnType = "String")]
        public virtual string XmlData
        {
            get
            {
                return this._xmlData;
            }
            set
            {
                this._xmlData = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "FileKey", ColumnType = "Int32")]
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
    }
}