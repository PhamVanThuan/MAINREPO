
using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("GenericKeyType", Schema = "dbo")]
    public partial class GenericKeyType_WTF_DAO : DB_Test_WTF<GenericKeyType_WTF_DAO>
    {

        private string _description;

        private string _tableName;

        private string _primaryKeyColumn;

        private int _key;

        [Property("Description", ColumnType = "String", NotNull = true)]
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

        [PrimaryKey(PrimaryKeyType.Native, "GenericKeyTypeKey", ColumnType = "Int32")]
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


