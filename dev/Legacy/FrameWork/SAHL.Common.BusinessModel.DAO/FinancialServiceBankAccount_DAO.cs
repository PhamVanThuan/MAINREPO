using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("FinancialServiceBankAccount", Schema = "dbo", Lazy = true)]
    public partial class FinancialServiceBankAccount_DAO : DB_2AM<FinancialServiceBankAccount_DAO>
    {
        private double _percentage;

        private int _debitOrderDay;

        private string _userID;

        private System.DateTime _changeDate;

        private int _Key;

        private BankAccount_DAO _bankAccount;

        private FinancialService_DAO _financialService;

        private FinancialServicePaymentType_DAO _financialServicePaymentType;

        private GeneralStatus_DAO _generalStatus;

        private PaymentSplitType_DAO _PaymentSplitType;

        private bool _isNaedoCompliant;

        private int? _providerKey;

        [Property("Percentage", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Percentage is a mandatory field")]
        public virtual double Percentage
        {
            get
            {
                return this._percentage;
            }
            set
            {
                this._percentage = value;
            }
        }

        [Lurker]
        [Property("DebitOrderDay", ColumnType = "Int32", NotNull = true)]
        public virtual int DebitOrderDay
        {
            get
            {
                return this._debitOrderDay;
            }
            set
            {
                this._debitOrderDay = value;
            }
        }

        [Property("UserID", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("User ID is a mandatory field")]
        public virtual string UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
            }
        }

        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Change Date is a mandatory field")]
        public virtual System.DateTime ChangeDate
        {
            get
            {
                return this._changeDate;
            }
            set
            {
                this._changeDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "FinancialServiceBankAccountKey", ColumnType = "Int32")]
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

        [BelongsTo("BankAccountKey", NotNull = false)]
        public virtual BankAccount_DAO BankAccount
        {
            get
            {
                return this._bankAccount;
            }
            set
            {
                this._bankAccount = value;
            }
        }

        [BelongsTo("FinancialServiceKey", NotNull = true)]
        [ValidateNonEmpty("Financial Service is a mandatory field")]
        public virtual FinancialService_DAO FinancialService
        {
            get
            {
                return this._financialService;
            }
            set
            {
                this._financialService = value;
            }
        }

        [BelongsTo("FinancialServicePaymentTypeKey", NotNull = true)]
        [ValidateNonEmpty("Financial Service Payment Type is a mandatory field")]
        public virtual FinancialServicePaymentType_DAO FinancialServicePaymentType
        {
            get
            {
                return this._financialServicePaymentType;
            }
            set
            {
                this._financialServicePaymentType = value;
            }
        }

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        [BelongsTo("PaymentSplitTypeKey")]
        public virtual PaymentSplitType_DAO PaymentSplitType
        {
            get
            {
                return this._PaymentSplitType;
            }
            set
            {
                this._PaymentSplitType = value;
            }
        }


        [Property("IsNaedoCompliant", ColumnType = "Boolean")]
        public virtual bool IsNaedoCompliant
        {
            get
            {
                return this._isNaedoCompliant;
            }
            set
            {
                this._isNaedoCompliant = value;
            }
        }

        [Property("ProviderKey", ColumnType = "Int32", NotNull=false)]
        public virtual int? ProviderKey
        {
            get
            {
                return this._providerKey;
            }
            set
            {
                this._providerKey = value;
            }
        }
    }
}