using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("WorkflowRuleSet", Schema = "dbo", Lazy = false)]
    public class WorkflowRuleSet_DAO : DB_2AM<WorkflowRuleSet_DAO>
    {
        int _Key;
        string _Name;

        IList<RuleItem_DAO> _Rules;

        [PrimaryKey(PrimaryKeyType.Native, "WorkflowRuleSetKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value;
            }
        }

        [Property("Name", ColumnType = "System.String")]
        public virtual string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        [HasAndBelongsToMany(typeof(RuleItem_DAO), Table = "WorkflowRuleSetItem", ColumnKey = "WorkflowRuleSetKey", ColumnRef = "RuleItemKey")]
        public virtual IList<RuleItem_DAO> Rules
        {
            get { return _Rules; }
            set { _Rules = value; }
        }
    }
}