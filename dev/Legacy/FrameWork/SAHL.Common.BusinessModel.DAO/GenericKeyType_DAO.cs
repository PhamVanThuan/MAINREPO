using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("GenericKeyType", Schema = "dbo", Lazy = false)]
    public partial class GenericKeyType_DAO : DB_2AM<GenericKeyType_DAO>
    {
        private string _description;

        private string _tableName;

        private string _primaryKeyColumn;

        private int _key;

        // Commented as this is a lookup object.
        // private IList<ConditionConfiguration_DAO> _conditionConfigurations;

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

        [Property("TableName", ColumnType = "String")]
        public virtual string TableName
        {
            get
            {
                return this._tableName;
            }
            set
            {
                this._tableName = value;
            }
        }

        [Property("PrimaryKeyColumn", ColumnType = "String")]
        public virtual string PrimaryKeyColumn
        {
            get
            {
                return this._primaryKeyColumn;
            }
            set
            {
                this._primaryKeyColumn = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "GenericKeyTypeKey", ColumnType = "Int32")]
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

        // Commented as this is a lookup object.
        //[HasMany(typeof(ConditionConfiguration_DAO), ColumnKey = "GenericKeyTypeKey", Lazy = true, Table = "ConditionConfiguration")]
        //public virtual IList<ConditionConfiguration_DAO> ConditionConfigurations
        //{
        //    get
        //    {
        //        return this._conditionConfigurations;
        //    }
        //    set
        //    {
        //        this._conditionConfigurations = value;
        //    }
        //}
    }
}