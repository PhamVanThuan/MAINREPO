using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Filter", Schema = "search")]
    public partial class Filter_DAO : DB_2AM<Filter_DAO>
    {
        private int _key;

        private string _name;

        private int _contextKey;

        private int _roleKey;

        private int _workflowContextKey;

        private string _query;

        private string _parameters;

        [PrimaryKey(PrimaryKeyType.Native, "FilterKey", ColumnType = "Int32")]
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

        [Property("ContextKey", ColumnType = "Int32")]
        public virtual int ContextKey
        {
            get
            {
                return this._contextKey;
            }
            set
            {
                this._contextKey = value;
            }
        }

        [Property("RoleKey", ColumnType = "Int32")]
        public virtual int RoleKey
        {
            get
            {
                return this._roleKey;
            }
            set
            {
                this._roleKey = value;
            }
        }

        [Property("WorkflowContextKey", ColumnType = "Int32")]
        public virtual int WorkflowContextKey
        {
            get
            {
                return this._workflowContextKey;
            }
            set
            {
                this._workflowContextKey = value;
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

        [Property("Parameters", ColumnType = "String")]
        public virtual string Parameters
        {
            get
            {
                return this._parameters;
            }
            set
            {
                this._parameters = value;
            }
        }
    }
}