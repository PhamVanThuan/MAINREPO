using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("DisbursementTransactionType", Schema = "dbo", Lazy = true)]
    public partial class DisbursementTransactionType_DAO : DB_2AM<DisbursementTransactionType_DAO>
    {
        private string _description;

        private int? _transactionTypeNumber;

        private int _Key;

        // todo: Check if this relationship is needed
        //private IList<Disbursement_DAO> _disbursements;

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
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

        [Property("TransactionTypeNumber", ColumnType = "Int32")]
        public virtual int? TransactionTypeNumber
        {
            get
            {
                return this._transactionTypeNumber;
            }
            set
            {
                this._transactionTypeNumber = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "DisbursementTransactionTypeKey", ColumnType = "Int32")]
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

        // todo: check to see if this list is needed
        //[HasMany(typeof(Disbursement_DAO), ColumnKey = "DisbursementTransactionTypeKey", Table = "Disbursement")]
        //public virtual IList<Disbursement_DAO> Disbursements
        //{
        //    get
        //    {
        //        return this._disbursements;
        //    }
        //    set
        //    {
        //        this._disbursements = value;
        //    }
        //}
    }
}