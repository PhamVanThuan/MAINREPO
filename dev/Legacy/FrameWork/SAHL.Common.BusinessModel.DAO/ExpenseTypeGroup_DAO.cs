using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ExpenseTypeGroup", Schema = "dbo")]
    public partial class ExpenseTypeGroup_DAO : DB_2AM<ExpenseTypeGroup_DAO>
    {
        private string _description;

        private bool? _fee;

        private bool? _expense;

        private int _Key;

        private IList<ExpenseType_DAO> _expenseTypes;

        [Property("Description", ColumnType = "String")]
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

        [Property("Fee", ColumnType = "Boolean")]
        public virtual bool? Fee
        {
            get
            {
                return this._fee;
            }
            set
            {
                this._fee = value;
            }
        }

        [Property("Expense", ColumnType = "Boolean")]
        public virtual bool? Expense
        {
            get
            {
                return this._expense;
            }
            set
            {
                this._expense = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ExpenseTypeGroupKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        [HasMany(typeof(ExpenseType_DAO), ColumnKey = "ExpenseTypeGroupKey", Table = "ExpenseType", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ExpenseType_DAO> ExpenseTypes
        {
            get
            {
                return this._expenseTypes;
            }
            set
            {
                this._expenseTypes = value;
            }
        }
    }
}