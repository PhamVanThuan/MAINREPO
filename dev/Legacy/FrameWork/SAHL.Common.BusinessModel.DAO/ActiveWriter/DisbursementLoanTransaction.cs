﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SAHL.Common.BusinessModel {
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using Castle.ActiveRecord;
    
    
    [ActiveRecord("DisbursementLoanTransaction", Schema="dbo")]
    public partial class DisbursementLoanTransaction : ActiveRecordBase<DisbursementLoanTransaction> {
        
        private int _loanTransactionNumber;
        
        private int _disbursementLoanTransactionKey;
        
        private Disbursement _disbursement;
        
        [Property("LoanTransactionNumber", ColumnType="Int32", NotNull=true)]
        public virtual int LoanTransactionNumber {
            get {
                return this._loanTransactionNumber;
            }
            set {
                this._loanTransactionNumber = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "DisbursementLoanTransactionKey", ColumnType="Int32")]
        public virtual int DisbursementLoanTransactionKey {
            get {
                return this._disbursementLoanTransactionKey;
            }
            set {
                this._disbursementLoanTransactionKey = value;
            }
        }
        
        [BelongsTo("DisbursementKey")]
        public virtual Disbursement Disbursement {
            get {
                return this._disbursement;
            }
            set {
                this._disbursement = value;
            }
        }
    }
    
    [ActiveRecord("Disbursement", Schema="dbo")]
    public partial class Disbursement : ActiveRecordBase<Disbursement> {
        
        private int _accountKey;
        
        private int _aCBBankCode;
        
        private string _aCBBranchCode;
        
        private int _aCBTypeNumber;
        
        private System.DateTime _preparedDate;
        
        private System.DateTime _actionDate;
        
        private string _accountName;
        
        private string _accountNumber;
        
        private double _amount;
        
        private double _capitalAmount;
        
        private double _guaranteeAmount;
        
        private double _interestRate;
        
        private System.DateTime _interestStartDate;
        
        private string _interestApplied;
        
        private double _paymentAmount;
        
        private int _disbursementKey;
        
        private IList<DisbursementLoanTransaction> _disbursementLoanTransactions;
        
        private DisbursementStatus _disbursementStatus;
        
        private DisbursementTransactionType _disbursementTransactionType;
        
        [Property("AccountKey", ColumnType="Int32", NotNull=true)]
        public virtual int AccountKey {
            get {
                return this._accountKey;
            }
            set {
                this._accountKey = value;
            }
        }
        
        [Property("ACBBankCode", ColumnType="Int32")]
        public virtual int ACBBankCode {
            get {
                return this._aCBBankCode;
            }
            set {
                this._aCBBankCode = value;
            }
        }
        
        [Property("ACBBranchCode", ColumnType="String")]
        public virtual string ACBBranchCode {
            get {
                return this._aCBBranchCode;
            }
            set {
                this._aCBBranchCode = value;
            }
        }
        
        [Property("ACBTypeNumber", ColumnType="Int32")]
        public virtual int ACBTypeNumber {
            get {
                return this._aCBTypeNumber;
            }
            set {
                this._aCBTypeNumber = value;
            }
        }
        
        [Property("PreparedDate", ColumnType="Timestamp")]
        public virtual System.DateTime PreparedDate {
            get {
                return this._preparedDate;
            }
            set {
                this._preparedDate = value;
            }
        }
        
        [Property("ActionDate", ColumnType="Timestamp")]
        public virtual System.DateTime ActionDate {
            get {
                return this._actionDate;
            }
            set {
                this._actionDate = value;
            }
        }
        
        [Property("AccountName", ColumnType="String")]
        public virtual string AccountName {
            get {
                return this._accountName;
            }
            set {
                this._accountName = value;
            }
        }
        
        [Property("AccountNumber", ColumnType="String")]
        public virtual string AccountNumber {
            get {
                return this._accountNumber;
            }
            set {
                this._accountNumber = value;
            }
        }
        
        [Property("Amount", ColumnType="Double")]
        public virtual double Amount {
            get {
                return this._amount;
            }
            set {
                this._amount = value;
            }
        }
        
        [Property("CapitalAmount", ColumnType="Double")]
        public virtual double CapitalAmount {
            get {
                return this._capitalAmount;
            }
            set {
                this._capitalAmount = value;
            }
        }
        
        [Property("GuaranteeAmount", ColumnType="Double")]
        public virtual double GuaranteeAmount {
            get {
                return this._guaranteeAmount;
            }
            set {
                this._guaranteeAmount = value;
            }
        }
        
        [Property("InterestRate", ColumnType="Double")]
        public virtual double InterestRate {
            get {
                return this._interestRate;
            }
            set {
                this._interestRate = value;
            }
        }
        
        [Property("InterestStartDate", ColumnType="Timestamp")]
        public virtual System.DateTime InterestStartDate {
            get {
                return this._interestStartDate;
            }
            set {
                this._interestStartDate = value;
            }
        }
        
        [Property("InterestApplied", ColumnType="AnsiChar")]
        public virtual string InterestApplied {
            get {
                return this._interestApplied;
            }
            set {
                this._interestApplied = value;
            }
        }
        
        [Property("PaymentAmount", ColumnType="Double")]
        public virtual double PaymentAmount {
            get {
                return this._paymentAmount;
            }
            set {
                this._paymentAmount = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "DisbursementKey", ColumnType="Int32")]
        public virtual int DisbursementKey {
            get {
                return this._disbursementKey;
            }
            set {
                this._disbursementKey = value;
            }
        }
        
        [HasMany(typeof(DisbursementLoanTransaction), ColumnKey="DisbursementKey", Table="DisbursementLoanTransaction")]
        public virtual IList<DisbursementLoanTransaction> DisbursementLoanTransactions {
            get {
                return this._disbursementLoanTransactions;
            }
            set {
                this._disbursementLoanTransactions = value;
            }
        }
        
        [BelongsTo("DisbursementStatusKey")]
        public virtual DisbursementStatus DisbursementStatus {
            get {
                return this._disbursementStatus;
            }
            set {
                this._disbursementStatus = value;
            }
        }
        
        [BelongsTo("DisbursementTransactionTypeKey")]
        public virtual DisbursementTransactionType DisbursementTransactionType {
            get {
                return this._disbursementTransactionType;
            }
            set {
                this._disbursementTransactionType = value;
            }
        }
    }
    
    [ActiveRecord("DisbursementStatus", Schema="dbo")]
    public partial class DisbursementStatus : ActiveRecordBase<DisbursementStatus> {
        
        private string _description;
        
        private int _disbursementStatusKey;
        
        private IList<Disbursement> _disbursements;
        
        [Property("Description", ColumnType="String", NotNull=true)]
        public virtual string Description {
            get {
                return this._description;
            }
            set {
                this._description = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "DisbursementStatusKey", ColumnType="Int32")]
        public virtual int DisbursementStatusKey {
            get {
                return this._disbursementStatusKey;
            }
            set {
                this._disbursementStatusKey = value;
            }
        }
        
        [HasMany(typeof(Disbursement), ColumnKey="DisbursementStatusKey", Table="Disbursement")]
        public virtual IList<Disbursement> Disbursements {
            get {
                return this._disbursements;
            }
            set {
                this._disbursements = value;
            }
        }
    }
    
    [ActiveRecord("DisbursementTransactionType", Schema="dbo")]
    public partial class DisbursementTransactionType : ActiveRecordBase<DisbursementTransactionType> {
        
        private string _description;
        
        private int _transactionTypeNumber;
        
        private int _disbursementTransactionTypeKey;
        
        private IList<Disbursement> _disbursements;
        
        [Property("Description", ColumnType="String", NotNull=true)]
        public virtual string Description {
            get {
                return this._description;
            }
            set {
                this._description = value;
            }
        }
        
        [Property("TransactionTypeNumber", ColumnType="Int32")]
        public virtual int TransactionTypeNumber {
            get {
                return this._transactionTypeNumber;
            }
            set {
                this._transactionTypeNumber = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "DisbursementTransactionTypeKey", ColumnType="Int32")]
        public virtual int DisbursementTransactionTypeKey {
            get {
                return this._disbursementTransactionTypeKey;
            }
            set {
                this._disbursementTransactionTypeKey = value;
            }
        }
        
        [HasMany(typeof(Disbursement), ColumnKey="DisbursementTransactionTypeKey", Table="Disbursement")]
        public virtual IList<Disbursement> Disbursements {
            get {
                return this._disbursements;
            }
            set {
                this._disbursements = value;
            }
        }
    }
    
    [ActiveRecord("DisbursementType", Schema="dbo")]
    public partial class DisbursementType : ActiveRecordBase<DisbursementType> {
        
        private string _description;
        
        private int _disbursementTypeKey;
        
        [Property("Description", ColumnType="String")]
        public virtual string Description {
            get {
                return this._description;
            }
            set {
                this._description = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "DisbursementTypeKey", ColumnType="Int32")]
        public virtual int DisbursementTypeKey {
            get {
                return this._disbursementTypeKey;
            }
            set {
                this._disbursementTypeKey = value;
            }
        }
    }
    
    [ActiveRecord("DisbursementInterestApplied", Schema="dbo")]
    public partial class DisbursementInterestApplied : ActiveRecordBase<DisbursementInterestApplied> {
        
        private string _description;
        
        private int _interestAppliedTypeKey;
        
        [Property("Description", ColumnType="String")]
        public virtual string Description {
            get {
                return this._description;
            }
            set {
                this._description = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Native, "InterestAppliedTypeKey", ColumnType="Int32")]
        public virtual int InterestAppliedTypeKey {
            get {
                return this._interestAppliedTypeKey;
            }
            set {
                this._interestAppliedTypeKey = value;
            }
        }
    }
}
