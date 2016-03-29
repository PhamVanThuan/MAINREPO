using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("WorkflowData", Schema = "search")]
    public partial class WorkflowData_DAO : DB_2AM<WorkflowData_DAO>
    {
        private int _key;

        private string _tableName;

        private string _alias;

        private string _primaryKeyColumn;

        private int _workflowContextKey;

        private string _foreignKeyColumn;

        [PrimaryKey(PrimaryKeyType.Native, "WorkflowDataKey", ColumnType = "Int32")]
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

        [Property("ForeignKeyColumn", ColumnType = "String")]
        public virtual string ForeignKeyColumn
        {
            get
            {
                return this._foreignKeyColumn;
            }
            set
            {
                this._foreignKeyColumn = value;
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
    }
}