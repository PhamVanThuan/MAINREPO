﻿using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("InternalRole", Schema = "search")]
    public partial class InternalRole_DAO : DB_2AM<InternalRole_DAO>
    {
        private int _key;

        private string _tableName;

        private string _alias;

        private string _primaryKeyColumn;

        private int _contextTableKey;

        private string _contextTableJoinKey;

        [PrimaryKey(PrimaryKeyType.Native, "InternalRoleKey", ColumnType = "Int32")]
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

        [Property("Alias", ColumnType = "String")]
        public virtual string Alias
        {
            get
            {
                return this._alias;
            }
            set
            {
                this._alias = value;
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

        [Property("ContextTableKey", ColumnType = "Int32")]
        public virtual int ContextTableKey
        {
            get
            {
                return this._contextTableKey;
            }
            set
            {
                this._contextTableKey = value;
            }
        }

        [Property("ContextTableJoinKey", ColumnType = "String")]
        public virtual string ContextTableJoinKey
        {
            get
            {
                return this._contextTableJoinKey;
            }
            set
            {
                this._contextTableJoinKey = value;
            }
        }
    }
}