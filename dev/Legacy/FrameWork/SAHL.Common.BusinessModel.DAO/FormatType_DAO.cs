using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("FormatType", Schema = "dbo")]
    public partial class FormatType_DAO : DB_2AM<FormatType_DAO>
    {
        private string _description;

        private string _format;

        private int _key;

        // commented, this is a lookup.
        //private IList<DataGridConfiguration> _dataGridConfigurations;

        // commented, this is a lookup.
        //private IList<DomainField> _domainFields;

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

        [Property("Format", ColumnType = "String", NotNull = true, Length = 255)]
        [ValidateNonEmpty("Format is a mandatory field")]
        public virtual string Format
        {
            get
            {
                return this._format;
            }
            set
            {
                this._format = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "FormatTypeKey", ColumnType = "Int32")]
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

        // commented, this is a lookup.
        //[HasMany(typeof(DataGridConfiguration), ColumnKey = "FormatTypeKey", Table = "DataGridConfiguration")]
        //public virtual IList<DataGridConfiguration> DataGridConfigurations
        //{
        //    get
        //    {
        //        return this._dataGridConfigurations;
        //    }
        //    set
        //    {
        //        this._dataGridConfigurations = value;
        //    }
        //}

        // commented, this is a lookup.
        //[HasMany(typeof(DomainField), ColumnKey = "FormatTypeKey", Table = "DomainField")]
        //public virtual IList<DomainField> DomainFields
        //{
        //    get
        //    {
        //        return this._domainFields;
        //    }
        //    set
        //    {
        //        this._domainFields = value;
        //    }
        //}
    }
}