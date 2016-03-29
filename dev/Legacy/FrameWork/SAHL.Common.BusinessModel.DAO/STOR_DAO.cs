using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("STOR", Schema = "dbo")]
    public partial class STOR_DAO : DB_ImageIndex<STOR_DAO>
    {
        private string _name;

        private string _description;

        private string _folder;

        private int _bFulltext;

        private string _nonIndexableChars;

        private string _key1;

        private string _key2;

        private string _key3;

        private string _key4;

        private string _key5;

        private string _key6;

        private string _key7;

        private string _key8;

        private string _key1Options;

        private string _key2Options;

        private string _key3Options;

        private string _key4Options;

        private string _key5Options;

        private string _key6Options;

        private string _key7Options;

        private string _key8Options;

        private string _key1MinMax;

        private string _key2MinMax;

        private string _key3MinMax;

        private string _key4MinMax;

        private string _key5MinMax;

        private string _key6MinMax;

        private string _key7MinMax;

        private string _key8MinMax;

        private int _bAudit;

        private string _logFolder;

        private string _defaultDocTitle;

        private string _key9;

        private string _key10;

        private string _key11;

        private string _key12;

        private string _key13;

        private string _key14;

        private string _key15;

        private string _key16;

        private string _key9Options;

        private string _key10Options;

        private string _key11Options;

        private string _key12Options;

        private string _key13Options;

        private string _key14Options;

        private string _key15Options;

        private string _key16Options;

        private string _key9MinMax;

        private string _key10MinMax;

        private string _key11MinMax;

        private string _key12MinMax;

        private string _key13MinMax;

        private string _key14MinMax;

        private string _key15MinMax;

        private string _key16MinMax;

        private string _exclusions;

        private decimal _key;

        [Property("Name", ColumnType = "String")]
        public virtual string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

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

        [Property("Folder", ColumnType = "String")]
        public virtual string Folder
        {
            get
            {
                return this._folder;
            }
            set
            {
                this._folder = value;
            }
        }

        [Property("bFulltext", ColumnType = "Int32")]
        public virtual int BFulltext
        {
            get
            {
                return this._bFulltext;
            }
            set
            {
                this._bFulltext = value;
            }
        }

        [Property("NonIndexableChars", ColumnType = "String")]
        public virtual string NonIndexableChars
        {
            get
            {
                return this._nonIndexableChars;
            }
            set
            {
                this._nonIndexableChars = value;
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

        [Property("Key1Options", ColumnType = "String")]
        public virtual string Key1Options
        {
            get
            {
                return this._key1Options;
            }
            set
            {
                this._key1Options = value;
            }
        }

        [Property("Key2Options", ColumnType = "String")]
        public virtual string Key2Options
        {
            get
            {
                return this._key2Options;
            }
            set
            {
                this._key2Options = value;
            }
        }

        [Property("Key3Options", ColumnType = "String")]
        public virtual string Key3Options
        {
            get
            {
                return this._key3Options;
            }
            set
            {
                this._key3Options = value;
            }
        }

        [Property("Key4Options", ColumnType = "String")]
        public virtual string Key4Options
        {
            get
            {
                return this._key4Options;
            }
            set
            {
                this._key4Options = value;
            }
        }

        [Property("Key5Options", ColumnType = "String")]
        public virtual string Key5Options
        {
            get
            {
                return this._key5Options;
            }
            set
            {
                this._key5Options = value;
            }
        }

        [Property("Key6Options", ColumnType = "String")]
        public virtual string Key6Options
        {
            get
            {
                return this._key6Options;
            }
            set
            {
                this._key6Options = value;
            }
        }

        [Property("Key7Options", ColumnType = "String")]
        public virtual string Key7Options
        {
            get
            {
                return this._key7Options;
            }
            set
            {
                this._key7Options = value;
            }
        }

        [Property("Key8Options", ColumnType = "String")]
        public virtual string Key8Options
        {
            get
            {
                return this._key8Options;
            }
            set
            {
                this._key8Options = value;
            }
        }

        [Property("Key1MinMax", ColumnType = "String")]
        public virtual string Key1MinMax
        {
            get
            {
                return this._key1MinMax;
            }
            set
            {
                this._key1MinMax = value;
            }
        }

        [Property("Key2MinMax", ColumnType = "String")]
        public virtual string Key2MinMax
        {
            get
            {
                return this._key2MinMax;
            }
            set
            {
                this._key2MinMax = value;
            }
        }

        [Property("Key3MinMax", ColumnType = "String")]
        public virtual string Key3MinMax
        {
            get
            {
                return this._key3MinMax;
            }
            set
            {
                this._key3MinMax = value;
            }
        }

        [Property("Key4MinMax", ColumnType = "String")]
        public virtual string Key4MinMax
        {
            get
            {
                return this._key4MinMax;
            }
            set
            {
                this._key4MinMax = value;
            }
        }

        [Property("Key5MinMax", ColumnType = "String")]
        public virtual string Key5MinMax
        {
            get
            {
                return this._key5MinMax;
            }
            set
            {
                this._key5MinMax = value;
            }
        }

        [Property("Key6MinMax", ColumnType = "String")]
        public virtual string Key6MinMax
        {
            get
            {
                return this._key6MinMax;
            }
            set
            {
                this._key6MinMax = value;
            }
        }

        [Property("Key7MinMax", ColumnType = "String")]
        public virtual string Key7MinMax
        {
            get
            {
                return this._key7MinMax;
            }
            set
            {
                this._key7MinMax = value;
            }
        }

        [Property("Key8MinMax", ColumnType = "String")]
        public virtual string Key8MinMax
        {
            get
            {
                return this._key8MinMax;
            }
            set
            {
                this._key8MinMax = value;
            }
        }

        [Property("BAudit", ColumnType = "Int32")]
        public virtual int BAudit
        {
            get
            {
                return this._bAudit;
            }
            set
            {
                this._bAudit = value;
            }
        }

        [Property("LogFolder", ColumnType = "String")]
        public virtual string LogFolder
        {
            get
            {
                return this._logFolder;
            }
            set
            {
                this._logFolder = value;
            }
        }

        [Property("DefaultDocTitle", ColumnType = "String")]
        public virtual string DefaultDocTitle
        {
            get
            {
                return this._defaultDocTitle;
            }
            set
            {
                this._defaultDocTitle = value;
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

        [Property("Key9Options", ColumnType = "String")]
        public virtual string Key9Options
        {
            get
            {
                return this._key9Options;
            }
            set
            {
                this._key9Options = value;
            }
        }

        [Property("Key10Options", ColumnType = "String")]
        public virtual string Key10Options
        {
            get
            {
                return this._key10Options;
            }
            set
            {
                this._key10Options = value;
            }
        }

        [Property("Key11Options", ColumnType = "String")]
        public virtual string Key11Options
        {
            get
            {
                return this._key11Options;
            }
            set
            {
                this._key11Options = value;
            }
        }

        [Property("Key12Options", ColumnType = "String")]
        public virtual string Key12Options
        {
            get
            {
                return this._key12Options;
            }
            set
            {
                this._key12Options = value;
            }
        }

        [Property("Key13Options", ColumnType = "String")]
        public virtual string Key13Options
        {
            get
            {
                return this._key13Options;
            }
            set
            {
                this._key13Options = value;
            }
        }

        [Property("Key14Options", ColumnType = "String")]
        public virtual string Key14Options
        {
            get
            {
                return this._key14Options;
            }
            set
            {
                this._key14Options = value;
            }
        }

        [Property("Key15Options", ColumnType = "String")]
        public virtual string Key15Options
        {
            get
            {
                return this._key15Options;
            }
            set
            {
                this._key15Options = value;
            }
        }

        [Property("Key16Options", ColumnType = "String")]
        public virtual string Key16Options
        {
            get
            {
                return this._key16Options;
            }
            set
            {
                this._key16Options = value;
            }
        }

        [Property("Key9MinMax", ColumnType = "String")]
        public virtual string Key9MinMax
        {
            get
            {
                return this._key9MinMax;
            }
            set
            {
                this._key9MinMax = value;
            }
        }

        [Property("Key10MinMax", ColumnType = "String")]
        public virtual string Key10MinMax
        {
            get
            {
                return this._key10MinMax;
            }
            set
            {
                this._key10MinMax = value;
            }
        }

        [Property("Key11MinMax", ColumnType = "String")]
        public virtual string Key11MinMax
        {
            get
            {
                return this._key11MinMax;
            }
            set
            {
                this._key11MinMax = value;
            }
        }

        [Property("Key12MinMax", ColumnType = "String")]
        public virtual string Key12MinMax
        {
            get
            {
                return this._key12MinMax;
            }
            set
            {
                this._key12MinMax = value;
            }
        }

        [Property("Key13MinMax", ColumnType = "String")]
        public virtual string Key13MinMax
        {
            get
            {
                return this._key13MinMax;
            }
            set
            {
                this._key13MinMax = value;
            }
        }

        [Property("Key14MinMax", ColumnType = "String")]
        public virtual string Key14MinMax
        {
            get
            {
                return this._key14MinMax;
            }
            set
            {
                this._key14MinMax = value;
            }
        }

        [Property("Key15MinMax", ColumnType = "String")]
        public virtual string Key15MinMax
        {
            get
            {
                return this._key15MinMax;
            }
            set
            {
                this._key15MinMax = value;
            }
        }

        [Property("Key16MinMax", ColumnType = "String")]
        public virtual string Key16MinMax
        {
            get
            {
                return this._key16MinMax;
            }
            set
            {
                this._key16MinMax = value;
            }
        }

        [Property("Exclusions", ColumnType = "String")]
        public virtual string Exclusions
        {
            get
            {
                return this._exclusions;
            }
            set
            {
                this._exclusions = value;
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