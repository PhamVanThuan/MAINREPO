using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AllocationMandateOperator", Schema = "dbo")]
    public class AllocationMandateOperator_DAO : DB_2AM<AllocationMandateOperator_DAO>
    {
        private int _order;

        private int _key;

        private Operator_DAO _operator;

        private AllocationMandateSet_DAO _allocationMandateSet;

        private AllocationMandate_DAO _allocationMandate;

        [Property("`Order`", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Order is a mandatory field")]
        public virtual int Order
        {
            get
            {
                return this._order;
            }
            set
            {
                this._order = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "AllocationMandateOperatorKey", ColumnType = "Int32")]
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

        [BelongsTo("OperatorKey")]
        public virtual Operator_DAO AllocationOperator
        {
            get
            {
                return this._operator;
            }
            set
            {
                this._operator = value;
            }
        }

        [BelongsTo("AllocationMandateSetKey", NotNull = true)]
        [ValidateNonEmpty("Allocation Mandate Set is a mandatory field")]
        public virtual AllocationMandateSet_DAO AllocationMandateSet
        {
            get
            {
                return this._allocationMandateSet;
            }
            set
            {
                this._allocationMandateSet = value;
            }
        }

        [BelongsTo("AllocationMandateKey")]
        public virtual AllocationMandate_DAO AllocationMandate
        {
            get
            {
                return this._allocationMandate;
            }
            set
            {
                this._allocationMandate = value;
            }
        }
    }
}