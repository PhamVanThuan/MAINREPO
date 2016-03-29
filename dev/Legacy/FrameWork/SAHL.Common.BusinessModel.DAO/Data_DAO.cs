using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Data", Schema = "dbo")]
    public partial class Data_DAO : DB_ImageIndex<Data_DAO>
    {
        private string _securityGroup;

        private string _archiveDate;

        private decimal _dataContainer;

        private decimal _backupVolume;

        private string _overlay;

        private decimal _sTOR;

        private string _gUID;

        private string _extension;

        private string _key1;

        private string _key2;

        private string _key3;

        private string _key4;

        private string _key5;

        private string _key6;

        private string _key7;

        private string _key8;

        private string _msgTo;

        private string _msgFrom;

        private string _msgSubject;

        private System.DateTime? _msgReceived;

        private System.DateTime? _msgSent;

        private string _key9;

        private string _key10;

        private string _key11;

        private string _key12;

        private string _key13;

        private string _key14;

        private string _key15;

        private string _key16;

        private string _title;

        private string _originalFilename;

        private decimal _key;

        [Property("securityGroup", ColumnType = "String")]
        public virtual string SecurityGroup
        {
            get
            {
                return this._securityGroup;
            }
            set
            {
                this._securityGroup = value;
            }
        }

        [Property("archiveDate", ColumnType = "String")]
        public virtual string ArchiveDate
        {
            get
            {
                return this._archiveDate;
            }
            set
            {
                this._archiveDate = value;
            }
        }

        [Property("dataContainer", ColumnType = "Decimal")]
        public virtual decimal DataContainer
        {
            get
            {
                return this._dataContainer;
            }
            set
            {
                this._dataContainer = value;
            }
        }

        [Property("backupVolume", ColumnType = "Decimal")]
        public virtual decimal BackupVolume
        {
            get
            {
                return this._backupVolume;
            }
            set
            {
                this._backupVolume = value;
            }
        }

        [Property("Overlay", ColumnType = "String")]
        public virtual string Overlay
        {
            get
            {
                return this._overlay;
            }
            set
            {
                this._overlay = value;
            }
        }

        [Property("STOR", ColumnType = "Decimal")]
        public virtual decimal STOR
        {
            get
            {
                return this._sTOR;
            }
            set
            {
                this._sTOR = value;
            }
        }

        [Property("GUID", ColumnType = "String")]
        public virtual string GUID
        {
            get
            {
                return this._gUID;
            }
            set
            {
                this._gUID = value;
            }
        }

        [Property("Extension", ColumnType = "String")]
        public virtual string Extension
        {
            get
            {
                return this._extension;
            }
            set
            {
                this._extension = value;
            }
        }

        [Property("Key1", ColumnType = "String")]
        public virtual string Key1
        {
            get
            {
                return this._key1;
            }
            set
            {
                this._key1 = value;
            }
        }

        [Property("Key2", ColumnType = "String")]
        public virtual string Key2
        {
            get
            {
                return this._key2;
            }
            set
            {
                this._key2 = value;
            }
        }

        [Property("Key3", ColumnType = "String")]
        public virtual string Key3
        {
            get
            {
                return this._key3;
            }
            set
            {
                this._key3 = value;
            }
        }

        [Property("Key4", ColumnType = "String")]
        public virtual string Key4
        {
            get
            {
                return this._key4;
            }
            set
            {
                this._key4 = value;
            }
        }

        [Property("Key5", ColumnType = "String")]
        public virtual string Key5
        {
            get
            {
                return this._key5;
            }
            set
            {
                this._key5 = value;
            }
        }

        [Property("Key6", ColumnType = "String")]
        public virtual string Key6
        {
            get
            {
                return this._key6;
            }
            set
            {
                this._key6 = value;
            }
        }

        [Property("Key7", ColumnType = "String")]
        public virtual string Key7
        {
            get
            {
                return this._key7;
            }
            set
            {
                this._key7 = value;
            }
        }

        [Property("Key8", ColumnType = "String")]
        public virtual string Key8
        {
            get
            {
                return this._key8;
            }
            set
            {
                this._key8 = value;
            }
        }

        [Property("msgTo", ColumnType = "String")]
        public virtual string MsgTo
        {
            get
            {
                return this._msgTo;
            }
            set
            {
                this._msgTo = value;
            }
        }

        [Property("msgFrom", ColumnType = "String")]
        public virtual string MsgFrom
        {
            get
            {
                return this._msgFrom;
            }
            set
            {
                this._msgFrom = value;
            }
        }

        [Property("msgSubject", ColumnType = "String")]
        public virtual string MsgSubject
        {
            get
            {
                return this._msgSubject;
            }
            set
            {
                this._msgSubject = value;
            }
        }

        [Property("msgReceived", ColumnType = "Timestamp", NotNull = false)]
        public virtual System.DateTime? MsgReceived
        {
            get
            {
                return this._msgReceived;
            }
            set
            {
                this._msgReceived = value;
            }
        }

        [Property("msgSent", ColumnType = "Timestamp", NotNull = false)]
        public virtual System.DateTime? MsgSent
        {
            get
            {
                return this._msgSent;
            }
            set
            {
                this._msgSent = value;
            }
        }

        [Property("Key9", ColumnType = "String")]
        public virtual string Key9
        {
            get
            {
                return this._key9;
            }
            set
            {
                this._key9 = value;
            }
        }

        [Property("Key10", ColumnType = "String")]
        public virtual string Key10
        {
            get
            {
                return this._key10;
            }
            set
            {
                this._key10 = value;
            }
        }

        [Property("Key11", ColumnType = "String")]
        public virtual string Key11
        {
            get
            {
                return this._key11;
            }
            set
            {
                this._key11 = value;
            }
        }

        [Property("Key12", ColumnType = "String")]
        public virtual string Key12
        {
            get
            {
                return this._key12;
            }
            set
            {
                this._key12 = value;
            }
        }

        [Property("Key13", ColumnType = "String")]
        public virtual string Key13
        {
            get
            {
                return this._key13;
            }
            set
            {
                this._key13 = value;
            }
        }

        [Property("Key14", ColumnType = "String")]
        public virtual string Key14
        {
            get
            {
                return this._key14;
            }
            set
            {
                this._key14 = value;
            }
        }

        [Property("Key15", ColumnType = "String")]
        public virtual string Key15
        {
            get
            {
                return this._key15;
            }
            set
            {
                this._key15 = value;
            }
        }

        [Property("Key16", ColumnType = "String")]
        public virtual string Key16
        {
            get
            {
                return this._key16;
            }
            set
            {
                this._key16 = value;
            }
        }

        [Property("Title", ColumnType = "String")]
        public virtual string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }

        [Property("OriginalFilename", ColumnType = "String")]
        public virtual string OriginalFilename
        {
            get
            {
                return this._originalFilename;
            }
            set
            {
                this._originalFilename = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ID", ColumnType = "Decimal")]
        public virtual decimal Key
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