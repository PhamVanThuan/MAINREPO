using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferDebitOrder", Schema = "dbo", Lazy = true)]
    public class ApplicationDebitOrder_DAO : DB_2AM<ApplicationDebitOrder_DAO>
    {
        private int _Key;
        private Application_DAO _Application;
        private BankAccount_DAO _BankAccount;
        private double _Percentage;
        private int _DebitOrderDay;
        private FinancialServicePaymentType_DAO _FinancialServicePaymentType;

        [PrimaryKey(PrimaryKeyType.Native, Column = "OfferDebitOrderKey", ColumnType = "Int32")]
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

        [Property("Percentage", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Percentage is a mandatory field")]
        public virtual double Percentage
        {
            get { return _Percentage; }
            set { _Percentage = value; }
        }

        [Property("DebitOrderDay", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Debit Order Day is a mandatory field")]
        public virtual int DebitOrderDay
        {
            get { return _DebitOrderDay; }
            set { _DebitOrderDay = value; }
        }

        [BelongsTo("BankAccountKey", NotNull = false)]
        public virtual BankAccount_DAO BankAccount
        {
            get { return _BankAccount; }
            set { _BankAccount = value; }
        }

        [BelongsTo("OfferKey", NotNull = true)]
        [ValidateNonEmpty("Application is a mandatory field")]
        public virtual Application_DAO Application
        {
            get { return _Application; }
            set { _Application = value; }
        }

        [BelongsTo("FinancialServicePaymentTypeKey", NotNull = true)]
        [ValidateNonEmpty("Financial Service Payment Type is a mandatory field")]
        public virtual FinancialServicePaymentType_DAO FinancialServicePaymentType
        {
            get { return _FinancialServicePaymentType; }
            set { _FinancialServicePaymentType = value; }
        }
    }
}