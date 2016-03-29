using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("PaymentType", Schema = "dbo")]
    public partial class PaymentType_DAO : DB_2AM<PaymentType_DAO>
    {
        private string _description;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "PaymentTypeKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ExpenseType_DAO), ColumnKey = "PaymentTypeKey", Table = "ExpenseType", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
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