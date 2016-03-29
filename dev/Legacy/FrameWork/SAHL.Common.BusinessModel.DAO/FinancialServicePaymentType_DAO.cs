using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("FinancialServicePaymentType", Schema = "dbo", Lazy = true)]
    public partial class FinancialServicePaymentType_DAO : DB_2AM<FinancialServicePaymentType_DAO>
    {
        private string _description;

        private int _Key;

        //private IList<FinancialServiceBankAccount_DAO> _financialServiceBankAccounts;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "FinancialServicePaymentTypeKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(FinancialServiceBankAccount_DAO), ColumnKey = "FinancialServicePaymentTypeKey",Lazy = true,Table = "FinancialServiceBankAccount")]
        //public virtual IList<FinancialServiceBankAccount_DAO> FinancialServiceBankAccounts
        //{
        //    get
        //    {
        //        return this._financialServiceBankAccounts;
        //    }
        //    set
        //    {
        //        this._financialServiceBankAccounts = value;
        //    }
        //}
    }
}