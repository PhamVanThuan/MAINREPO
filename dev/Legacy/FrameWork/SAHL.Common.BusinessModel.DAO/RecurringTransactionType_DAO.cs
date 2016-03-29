using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("RecurringTransactionType", Schema = "dbo")]
    public partial class RecurringTransactionType_DAO : DB_2AM<RecurringTransactionType_DAO>
    {
        private string _description;

        private int _Key;

        //private IList<FinancialServiceRecurringTransactions_DAO> _financialServiceRecurringTransactions;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "RecurringTransactionTypeKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(FinancialServiceRecurringTransactions_DAO), ColumnKey = "RecurringTransactionTypeKey", Table = "FinancialServiceRecurringTransactions")]
        //public virtual IList<FinancialServiceRecurringTransactions_DAO> FinancialServiceRecurringTransactions
        //{
        //    get
        //    {
        //        return this._financialServiceRecurringTransactions;
        //    }
        //    set
        //    {
        //        this._financialServiceRecurringTransactions = value;
        //    }
        //}
    }
}