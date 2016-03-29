using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("GenericColumnDefinition", Schema = "dbo")]
    public partial class GenericColumnDefinition_DAO : DB_2AM<GenericColumnDefinition_DAO>
    {
        private string _description;

        private string _tableName;

        private string _columnName;

        private int _key;

        private IList<ConditionConfiguration_DAO> _conditionConfigurations;

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

        [Property("TableName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Table Name is a mandatory field")]
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

        [PrimaryKey(PrimaryKeyType.Native, "GenericColumnDefinitionKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ConditionConfiguration_DAO), ColumnKey = "GenericColumnDefinitionKey", Lazy = true, Table = "ConditionConfiguration")]
        public virtual IList<ConditionConfiguration_DAO> ConditionConfigurations
        {
            get
            {
                return this._conditionConfigurations;
            }
            set
            {
                this._conditionConfigurations = value;
            }
        }
    }
}