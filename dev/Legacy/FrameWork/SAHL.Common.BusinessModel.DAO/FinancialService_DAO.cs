using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("FinancialService", Schema = "dbo", Lazy = true, CustomAccess = Constants.ReadonlyAccess), JoinedBase]
    public partial class FinancialService_DAO : DB_2AM<FinancialService_DAO>
    {
        private double _payment;

        private Trade_DAO _trade;

        private System.DateTime? _nextResetDate;

        protected int _key;

        private IList<FinancialServiceBankAccount_DAO> _financialServiceBankAccounts;

        private IList<FinancialServiceCondition_DAO> _financialServiceConditions;

        private IList<ManualDebitOrder_DAO> _manualDebitOrders;

        private IList<FinancialAdjustment_DAO> _financialAdjustments;

        private IList<FinancialTransaction_DAO> _financialTransactions;

        private Account_DAO _account;

        private AccountStatus_DAO _accountStatus;

        private Category_DAO _category;

        private FinancialServiceType_DAO _financialServiceType;

        private FinancialService_DAO _parentFinancialService;

        private IList<FinancialService_DAO> _financialServices;

        private IList<ArrearTransaction_DAO> _arrearTransactions;

        private IList<Fee_DAO> _fees;

        private System.DateTime? _openDate;

        private System.DateTime? _closeDate;

        //private MortgageLoan_DAO _mortgageLoan;

        private Balance_DAO _balance;

        private LifePolicy_DAO _lifepolicy;

        private IList<FinancialServiceAttribute_DAO> _financialServiceAttributes;

        [Property("Payment", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Payment is a mandatory field")]
        public virtual double Payment
        {
            get
            {
                return this._payment;
            }
            set
            {
                this._payment = value;
            }
        }

        [BelongsTo("TradeKey", NotNull = false)]
        public virtual Trade_DAO Trade
        {
            get { return _trade; }
            set { _trade = value; }
        }

        [Property("NextResetDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? NextResetDate
        {
            get
            {
                return this._nextResetDate;
            }
            set
            {
                this._nextResetDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "FinancialServiceKey", ColumnType = "Int32")]
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

        [HasMany(typeof(FinancialServiceBankAccount_DAO), ColumnKey = "FinancialServiceKey", Table = "FinancialServiceBankAccount", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<FinancialServiceBankAccount_DAO> FinancialServiceBankAccounts
        {
            get
            {
                return this._financialServiceBankAccounts;
            }
            set
            {
                this._financialServiceBankAccounts = value;
            }
        }

        [HasMany(typeof(FinancialServiceCondition_DAO), ColumnKey = "FinancialServiceKey", Table = "FinancialServiceCondition", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<FinancialServiceCondition_DAO> FinancialServiceConditions
        {
            get
            {
                return this._financialServiceConditions;
            }
            set
            {
                this._financialServiceConditions = value;
            }
        }

        [HasMany(typeof(ManualDebitOrder_DAO), ColumnKey = "FinancialServiceKey", Table = "ManualDebitOrder", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ManualDebitOrder_DAO> ManualDebitOrders
        {
            get
            {
                return this._manualDebitOrders;
            }
            set
            {
                this._manualDebitOrders = value;
            }
        }

        [HasMany(typeof(FinancialAdjustment_DAO), ColumnKey = "FinancialServiceKey", Table = "FinancialAdjustment", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<FinancialAdjustment_DAO> FinancialAdjustments
        {
            get
            {
                return this._financialAdjustments;
            }
            set
            {
                this._financialAdjustments = value;
            }
        }

        [HasMany(typeof(FinancialServiceAttribute_DAO), ColumnKey = "FinancialServiceKey", Table = "FinancialServiceAttribute")]
        public virtual IList<FinancialServiceAttribute_DAO> FinancialServiceAttributes
        {
            get
            {
                return this._financialServiceAttributes;
            }
            set
            {
                this._financialServiceAttributes = value;
            }
        }

        [BelongsTo("AccountKey", NotNull = true)]
        [ValidateNonEmpty("Account is a mandatory field")]
        public virtual Account_DAO Account
        {
            get
            {
                return this._account;
            }
            set
            {
                this._account = value;
            }
        }

        [BelongsTo("AccountStatusKey", NotNull = true)]
        [ValidateNonEmpty("Account Status is a mandatory field")]
        public virtual AccountStatus_DAO AccountStatus
        {
            get
            {
                return this._accountStatus;
            }
            set
            {
                this._accountStatus = value;
            }
        }

        [BelongsTo("CategoryKey", NotNull = false)]
        public virtual Category_DAO Category
        {
            get
            {
                return this._category;
            }
            set
            {
                this._category = value;
            }
        }

        [BelongsTo("FinancialServiceTypeKey", NotNull = true)]
        [ValidateNonEmpty("Financial Service Type is a mandatory field")]
        [Lurker]
        public virtual FinancialServiceType_DAO FinancialServiceType
        {
            get
            {
                return this._financialServiceType;
            }
            set
            {
                this._financialServiceType = value;
            }
        }

        [HasMany(typeof(FinancialTransaction_DAO), ColumnKey = "financialServiceKey", Table = "FinancialTransaction", Lazy = true, Cascade = ManyRelationCascadeEnum.SaveUpdate)]
        public virtual IList<FinancialTransaction_DAO> FinancialTransactions
        {
            get
            {
                return this._financialTransactions;
            }
            set
            {
                this._financialTransactions = value;
            }
        }

        [BelongsTo("ParentFinancialServiceKey")]
        public virtual FinancialService_DAO FinancialServiceParent
        {
            get
            {
                return this._parentFinancialService;
            }
            set
            {
                this._parentFinancialService = value;
            }
        }

        [Property("OpenDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? OpenDate
        {
            get
            {
                return this._openDate;
            }
            set
            {
                this._openDate = value;
            }
        }

        [Property("CloseDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? CloseDate
        {
            get
            {
                return this._closeDate;
            }
            set
            {
                this._closeDate = value;
            }
        }

        [HasMany(typeof(FinancialService_DAO), ColumnKey = "ParentFinancialServiceKey", Table = "FinancialService")]
        public virtual IList<FinancialService_DAO> FinancialServices
        {
            get
            {
                return this._financialServices;
            }
            set
            {
                this._financialServices = value;
            }
        }

        [OneToOne]
        public virtual Balance_DAO Balance
        {
            get
            {
                return this._balance;
            }
            set
            {
                this._balance = value;
            }
        }

        [HasMany(typeof(ArrearTransaction_DAO), ColumnKey = "FinancialServiceKey", Table = "ArrearTransaction", Lazy = true, Cascade = ManyRelationCascadeEnum.All, Inverse = true)]
        public virtual IList<ArrearTransaction_DAO> ArrearTransactions
        {
            get
            {
                return this._arrearTransactions;
            }
            set
            {
                this._arrearTransactions = value;
            }
        }

        [HasMany(typeof(Fee_DAO), ColumnKey = "FinancialServiceKey", Table = "Fee", Lazy = true, Cascade = ManyRelationCascadeEnum.All, Inverse = true)]
        public virtual IList<Fee_DAO> Fees
        {
            get
            {
                return this._fees;
            }
            set
            {
                this._fees = value;
            }
        }

        [OneToOne]
        public virtual LifePolicy_DAO LifePolicy
        {
            get
            {
                return this._lifepolicy;
            }
            set
            {
                this._lifepolicy = value;
            }
        }
    }
}