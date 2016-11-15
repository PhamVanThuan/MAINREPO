﻿using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Cleanup", Schema = "search")]
    public partial class Cleanup_DAO : DB_2AM<Cleanup_DAO>
    {
        private int _key;

        private string _name;

        private string _query;

        [PrimaryKey(PrimaryKeyType.Native, "CleanupKey", ColumnType = "Int32")]
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

        [Property("Query", ColumnType = "String")]
        public virtual string Query
        {
            get
            {
                return this._query;
            }
            set
            {
                this._query = value;
            }
        }
    }
}