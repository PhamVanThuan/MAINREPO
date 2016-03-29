using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("WatchListConfiguration", Schema = "dbo", Lazy = true)]
    public class WatchListConfiguration_DAO : DB_2AM<WatchListConfiguration_DAO>
    {
        private int _key;

        private string _processName;

        private string _workFlowName;

        private string _statementName;

        [PrimaryKey(PrimaryKeyType.Native, "WatchListConfigurationKey", ColumnType = "Int32")]
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

        [Property("ProcessName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Process Name is a mandatory field")]
        public virtual string ProcessName
        {
            get
            {
                return this._processName;
            }
            set
            {
                this._processName = value;
            }
        }

        [Property("WorkFlowName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Workflow Name is a mandatory field")]
        public virtual string WorkFlowName
        {
            get
            {
                return this._workFlowName;
            }
            set
            {
                this._workFlowName = value;
            }
        }

        [Property("StatementName", ColumnType = "String", NotNull = true)]
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
    }
}