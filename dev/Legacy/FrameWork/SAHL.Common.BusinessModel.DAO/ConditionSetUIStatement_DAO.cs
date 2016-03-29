using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ConditionSetUIStatement", Schema = "dbo")]
    public class ConditionSetUIStatement_DAO : DB_2AM<ConditionSetUIStatement_DAO>
    {
        private int _key;

        private ConditionSet_DAO _conditionSet;

        private string _uiStatementName;

        [PrimaryKey(PrimaryKeyType.Native, "ConditionSetUIStatementKey", ColumnType = "Int32")]
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

        [BelongsTo("ConditionSetKey", NotNull = true)]
        public virtual ConditionSet_DAO ConditionSet
        {
            get
            {
                return this._conditionSet;
            }
            set
            {
                this._conditionSet = value;
            }
        }

        [Property("UIStatementName", ColumnType = "String", NotNull = true)]
        public virtual string UIStatementName
        {
            get
            {
                return this._uiStatementName;
            }
            set
            {
                this._uiStatementName = value;
            }
        }
    }
}
