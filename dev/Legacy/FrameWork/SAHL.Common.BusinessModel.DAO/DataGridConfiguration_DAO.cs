using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("DataGridConfiguration", Schema = "dbo", Lazy = true)]
    public class DataGridConfiguration_DAO : DB_2AM<DataGridConfiguration_DAO>
    {
        private string _statementName;

        private string _columnName;

        private string _columnDescription;

        private int _sequence;

        private string _width;

        private bool _visible;

        private bool _indexIdentifier;

        private int _key;

        private DataGridConfigurationType_DAO _dataGridConfigurationType;

        private FormatType_DAO _formatType;

        [Property("StatementName", ColumnType = "String", NotNull = true, Length = 50)]
        [ValidateNonEmpty("Statement Name is a mandatory field")]
        public virtual string StatementName
        {
            get
            {
                return this._statementName;
            }
            set
            {
                this._statementName = value;
            }
        }

        [Property("ColumnName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Column Name is a mandatory field")]
        public virtual string ColumnName
        {
            get
            {
                return this._columnName;
            }
            set
            {
                this._columnName = value;
            }
        }

        [Property("ColumnDescription", ColumnType = "String", NotNull = true, Length = 100)]
        [ValidateNonEmpty("Column Description is a mandatory field")]
        public virtual string ColumnDescription
        {
            get
            {
                return this._columnDescription;
            }
            set
            {
                this._columnDescription = value;
            }
        }

        [Property("Sequence", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Sequence is a mandatory field")]
        public virtual int Sequence
        {
            get
            {
                return this._sequence;
            }
            set
            {
                this._sequence = value;
            }
        }

        [Property("Width", ColumnType = "String", NotNull = true, Length = 10)]
        [ValidateNonEmpty("Width is a mandatory field")]
        public virtual string Width
        {
            get
            {
                return this._width;
            }
            set
            {
                this._width = value;
            }
        }

        [Property("Visible", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Visible is a mandatory field")]
        public virtual bool Visible
        {
            get
            {
                return this._visible;
            }
            set
            {
                this._visible = value;
            }
        }

        [Property("IndexIdentifier", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("IndexIdentifier is a mandatory field")]
        public virtual bool IndexIdentifier
        {
            get
            {
                return this._indexIdentifier;
            }
            set
            {
                this._indexIdentifier = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "DataGridConfigurationKey", ColumnType = "Int32")]
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

        [BelongsTo("DataGridConfigurationTypeKey", NotNull = false)]
        public virtual DataGridConfigurationType_DAO DataGridConfigurationType
        {
            get
            {
                return this._dataGridConfigurationType;
            }
            set
            {
                this._dataGridConfigurationType = value;
            }
        }

        [BelongsTo("FormatTypeKey", NotNull = false)]
        public virtual FormatType_DAO FormatType
        {
            get
            {
                return this._formatType;
            }
            set
            {
                this._formatType = value;
            }
        }
    }
}