﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
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
    
    
    [ActiveRecord("DebtCounselling", Schema="migration")]
    public partial class MigrationDebtCounselling_DAO : ActiveRecordBase<MigrationDebtCounselling_DAO> {
        
        private int _debtCounsellingKey;
        
        private int _accountKey;
        
        private int _debtCounsellingConsultantKey;
        
        private int _debtCounsellorKey;
        
        private int _proposalTypeKey;
        
        private System.DateTime _dateOf171;
        
        private System.DateTime _reviewDate;
        
        private System.DateTime _courtOrderDate;
        
        private System.DateTime _terminateDate;
        
        private System.DateTime _sixtyDaysDate;
        
        private System.DateTime _approvalDate;
        
        private decimal _approvalAmount;
        
        private int _approvalUserKey;
        
        private System.DateTime _paymentReceivedDate;
        
        private IList<MigrationDebtCounsellingExternalRole_DAO> _debtCounsellingExternalRoles;
        
        private IList<MigrationDebtCounsellingInternalRole_DAO> _debtCounsellingInternalRoles;
        
        private IList<MigrationDebtCounsellingProposal_DAO> _debtCounsellingProposals;
        
        [PrimaryKey(PrimaryKeyType.Native, "DebtCounsellingKey", ColumnType="Int32")]
        public virtual int DebtCounsellingKey {
            get {
                return this._debtCounsellingKey;
            }
            set {
                this._debtCounsellingKey = value;
            }
        }
        
        [Property("AccountKey", ColumnType="Int32", NotNull=true)]
        public virtual int AccountKey {
            get {
                return this._accountKey;
            }
            set {
                this._accountKey = value;
            }
        }
        
        [Property("DebtCounsellingConsultantKey", ColumnType="Int32", NotNull=true)]
        public virtual int DebtCounsellingConsultantKey {
            get {
                return this._debtCounsellingConsultantKey;
            }
            set {
                this._debtCounsellingConsultantKey = value;
            }
        }
        
        [Property("DebtCounsellorKey", ColumnType="Int32", NotNull=true)]
        public virtual int DebtCounsellorKey {
            get {
                return this._debtCounsellorKey;
            }
            set {
                this._debtCounsellorKey = value;
            }
        }
        
        [Property("ProposalTypeKey", ColumnType="Int32", NotNull=true)]
        public virtual int ProposalTypeKey {
            get {
                return this._proposalTypeKey;
            }
            set {
                this._proposalTypeKey = value;
            }
        }
        
        [Property("DateOf171", ColumnType="Timestamp", NotNull=true)]
        public virtual System.DateTime DateOf171 {
            get {
                return this._dateOf171;
            }
            set {
                this._dateOf171 = value;
            }
        }
        
        [Property("ReviewDate", ColumnType="Timestamp")]
        public virtual System.DateTime ReviewDate {
            get {
                return this._reviewDate;
            }
            set {
                this._reviewDate = value;
            }
        }
        
        [Property("CourtOrderDate", ColumnType="Timestamp")]
        public virtual System.DateTime CourtOrderDate {
            get {
                return this._courtOrderDate;
            }
            set {
                this._courtOrderDate = value;
            }
        }
        
        [Property("TerminateDate", ColumnType="Timestamp")]
        public virtual System.DateTime TerminateDate {
            get {
                return this._terminateDate;
            }
            set {
                this._terminateDate = value;
            }
        }
        
        [Property("SixtyDaysDate", ColumnType="Timestamp")]
        public virtual System.DateTime SixtyDaysDate {
            get {
                return this._sixtyDaysDate;
            }
            set {
                this._sixtyDaysDate = value;
            }
        }
        
        [Property("ApprovalDate", ColumnType="Timestamp")]
        public virtual System.DateTime ApprovalDate {
            get {
                return this._approvalDate;
            }
            set {
                this._approvalDate = value;
            }
        }
        
        [Property("ApprovalAmount", ColumnType="Decimal")]
        public virtual decimal ApprovalAmount {
            get {
                return this._approvalAmount;
            }
            set {
                this._approvalAmount = value;
            }
        }
        
        [Property("ApprovalUserKey", ColumnType="Int32")]
        public virtual int ApprovalUserKey {
            get {
                return this._approvalUserKey;
            }
            set {
                this._approvalUserKey = value;
            }
        }
        
        [Property("PaymentReceivedDate", ColumnType="Timestamp", NotNull=true)]
        public virtual System.DateTime PaymentReceivedDate {
            get {
                return this._paymentReceivedDate;
            }
            set {
                this._paymentReceivedDate = value;
            }
        }
        
        [HasMany(typeof(MigrationDebtCounsellingExternalRole_DAO), ColumnKey="DebtCounsellingKey", Table="DebtCounsellingExternalRole")]
        public virtual IList<MigrationDebtCounsellingExternalRole_DAO> DebtCounsellingExternalRoles {
            get {
                return this._debtCounsellingExternalRoles;
            }
            set {
                this._debtCounsellingExternalRoles = value;
            }
        }
        
        [HasMany(typeof(MigrationDebtCounsellingInternalRole_DAO), ColumnKey="DebtCounsellingKey", Table="DebtCounsellingInternalRole")]
        public virtual IList<MigrationDebtCounsellingInternalRole_DAO> DebtCounsellingInternalRoles {
            get {
                return this._debtCounsellingInternalRoles;
            }
            set {
                this._debtCounsellingInternalRoles = value;
            }
        }
        
        [HasMany(typeof(MigrationDebtCounsellingProposal_DAO), ColumnKey="DebtCounsellingKey", Table="DebtCounsellingProposal")]
        public virtual IList<MigrationDebtCounsellingProposal_DAO> DebtCounsellingProposals {
            get {
                return this._debtCounsellingProposals;
            }
            set {
                this._debtCounsellingProposals = value;
            }
        }
    }
    
    [ActiveRecord("DebtCounsellingProposalItem", Schema="migration")]
    public partial class MigrationDebtCounsellingProposalItem_DAO : ActiveRecordBase<MigrationDebtCounsellingProposalItem_DAO> {
        
        private int _debtCounsellingProposalItemKey;
        
        private System.DateTime _startDate;
        
        private System.DateTime _endDate;
        
        private decimal _marketRateKey;
        
        private decimal _interestRate;
        
        private decimal _amount;
        
        private decimal _additionalAmount;
        
        private System.DateTime _createDate;
        
        private decimal _instalmentPercentage;
        
        private decimal _annualEscalation;
        
        private MigrationDebtCounsellingProposal_DAO _debtCounsellingProposal;
        
        [PrimaryKey(PrimaryKeyType.Native, "DebtCounsellingProposalItemKey", ColumnType="Int32")]
        public virtual int DebtCounsellingProposalItemKey {
            get {
                return this._debtCounsellingProposalItemKey;
            }
            set {
                this._debtCounsellingProposalItemKey = value;
            }
        }
        
        [Property("StartDate", ColumnType="Timestamp", NotNull=true)]
        public virtual System.DateTime StartDate {
            get {
                return this._startDate;
            }
            set {
                this._startDate = value;
            }
        }
        
        [Property("EndDate", ColumnType="Timestamp", NotNull=true)]
        public virtual System.DateTime EndDate {
            get {
                return this._endDate;
            }
            set {
                this._endDate = value;
            }
        }
        
        [Property("MarketRateKey", ColumnType="Decimal", NotNull=true)]
        public virtual decimal MarketRateKey {
            get {
                return this._marketRateKey;
            }
            set {
                this._marketRateKey = value;
            }
        }
        
        [Property("InterestRate", ColumnType="Decimal", NotNull=true)]
        public virtual decimal InterestRate {
            get {
                return this._interestRate;
            }
            set {
                this._interestRate = value;
            }
        }
        
        [Property("Amount", ColumnType="Decimal", NotNull=true)]
        public virtual decimal Amount {
            get {
                return this._amount;
            }
            set {
                this._amount = value;
            }
        }
        
        [Property("AdditionalAmount", ColumnType="Decimal", NotNull=true)]
        public virtual decimal AdditionalAmount {
            get {
                return this._additionalAmount;
            }
            set {
                this._additionalAmount = value;
            }
        }
        
        [Property("CreateDate", ColumnType="Timestamp", NotNull=true)]
        public virtual System.DateTime CreateDate {
            get {
                return this._createDate;
            }
            set {
                this._createDate = value;
            }
        }
        
        [Property("InstalmentPercentage", ColumnType="Decimal", NotNull=true)]
        public virtual decimal InstalmentPercentage {
            get {
                return this._instalmentPercentage;
            }
            set {
                this._instalmentPercentage = value;
            }
        }
        
        [Property("AnnualEscalation", ColumnType="Decimal", NotNull=true)]
        public virtual decimal AnnualEscalation {
            get {
                return this._annualEscalation;
            }
            set {
                this._annualEscalation = value;
            }
        }
        
        [BelongsTo("DebtCounsellingProposalKey")]
        public virtual MigrationDebtCounsellingProposal_DAO DebtCounsellingProposal {
            get {
                return this._debtCounsellingProposal;
            }
            set {
                this._debtCounsellingProposal = value;
            }
        }
    }
    
    [ActiveRecord("DebtCounsellingExternalRole", Schema="migration")]
    public partial class MigrationDebtCounsellingExternalRole_DAO : ActiveRecordBase<MigrationDebtCounsellingExternalRole_DAO> {
        
        private int _debtCounsellingExternalRoleKey;
        
        private int _legalEntityKey;
        
        private int _externalRoleTypeKey;
        
        private MigrationDebtCounselling_DAO _debtCounselling;
        
        [PrimaryKey(PrimaryKeyType.Native, "DebtCounsellingExternalRoleKey", ColumnType="Int32")]
        public virtual int DebtCounsellingExternalRoleKey {
            get {
                return this._debtCounsellingExternalRoleKey;
            }
            set {
                this._debtCounsellingExternalRoleKey = value;
            }
        }
        
        [Property("LegalEntityKey", ColumnType="Int32", NotNull=true)]
        public virtual int LegalEntityKey {
            get {
                return this._legalEntityKey;
            }
            set {
                this._legalEntityKey = value;
            }
        }
        
        [Property("ExternalRoleTypeKey", ColumnType="Int32", NotNull=true)]
        public virtual int ExternalRoleTypeKey {
            get {
                return this._externalRoleTypeKey;
            }
            set {
                this._externalRoleTypeKey = value;
            }
        }
        
        [BelongsTo("DebtCounsellingKey")]
        public virtual MigrationDebtCounselling_DAO DebtCounselling {
            get {
                return this._debtCounselling;
            }
            set {
                this._debtCounselling = value;
            }
        }
    }
    
    [ActiveRecord("DebtCounsellingInternalRole", Schema="migration")]
    public partial class MigrationDebtCounsellingInternalRole_DAO : ActiveRecordBase<MigrationDebtCounsellingInternalRole_DAO> {
        
        private int _debtCounsellingInternalRole;
        
        private int _userOrganisationStructureKey;
        
        private MigrationDebtCounselling_DAO _debtCounselling;
        
        [PrimaryKey(PrimaryKeyType.Native, "DebtCounsellingInternalRole", ColumnType="Int32")]
        public virtual int DebtCounsellingInternalRole {
            get {
                return this._debtCounsellingInternalRole;
            }
            set {
                this._debtCounsellingInternalRole = value;
            }
        }
        
        [Property("UserOrganisationStructureKey", ColumnType="Int32", NotNull=true)]
        public virtual int UserOrganisationStructureKey {
            get {
                return this._userOrganisationStructureKey;
            }
            set {
                this._userOrganisationStructureKey = value;
            }
        }
        
        [BelongsTo("DebtCounsellingKey")]
        public virtual MigrationDebtCounselling_DAO DebtCounselling {
            get {
                return this._debtCounselling;
            }
            set {
                this._debtCounselling = value;
            }
        }
    }
    
    [ActiveRecord("DebtCounsellingProposal", Schema="migration")]
    public partial class MigrationDebtCounsellingProposal_DAO : ActiveRecordBase<MigrationDebtCounsellingProposal_DAO> {
        
        private int _debtCounsellingProposalKey;
        
        private int _proposalStatusKey;
        
        private bool _hOCInclusive;
        
        private bool _lifeInclusive;
        
        private bool _acceptedProposal;
        
        private System.DateTime _proposalDate;
        
        private IList<MigrationDebtCounsellingProposalItem_DAO> _debtCounsellingProposalItems;
        
        private MigrationDebtCounselling_DAO _debtCounselling;
        
        [PrimaryKey(PrimaryKeyType.Native, "DebtCounsellingProposalKey", ColumnType="Int32")]
        public virtual int DebtCounsellingProposalKey {
            get {
                return this._debtCounsellingProposalKey;
            }
            set {
                this._debtCounsellingProposalKey = value;
            }
        }
        
        [Property("ProposalStatusKey", ColumnType="Int32", NotNull=true)]
        public virtual int ProposalStatusKey {
            get {
                return this._proposalStatusKey;
            }
            set {
                this._proposalStatusKey = value;
            }
        }
        
        [Property("HOCInclusive", ColumnType="Boolean", NotNull=true)]
        public virtual bool HOCInclusive {
            get {
                return this._hOCInclusive;
            }
            set {
                this._hOCInclusive = value;
            }
        }
        
        [Property("LifeInclusive", ColumnType="Boolean", NotNull=true)]
        public virtual bool LifeInclusive {
            get {
                return this._lifeInclusive;
            }
            set {
                this._lifeInclusive = value;
            }
        }
        
        [Property("AcceptedProposal", ColumnType="Boolean", NotNull=true)]
        public virtual bool AcceptedProposal {
            get {
                return this._acceptedProposal;
            }
            set {
                this._acceptedProposal = value;
            }
        }
        
        [Property("ProposalDate", ColumnType="Timestamp", NotNull=true)]
        public virtual System.DateTime ProposalDate {
            get {
                return this._proposalDate;
            }
            set {
                this._proposalDate = value;
            }
        }
        
        [HasMany(typeof(MigrationDebtCounsellingProposalItem_DAO), ColumnKey="DebtCounsellingProposalKey", Table="DebtCounsellingProposalItem")]
        public virtual IList<MigrationDebtCounsellingProposalItem_DAO> DebtCounsellingProposalItems {
            get {
                return this._debtCounsellingProposalItems;
            }
            set {
                this._debtCounsellingProposalItems = value;
            }
        }
        
        [BelongsTo("DebtCounsellingKey")]
        public virtual MigrationDebtCounselling_DAO DebtCounselling {
            get {
                return this._debtCounselling;
            }
            set {
                this._debtCounselling = value;
            }
        }
    }
    
    public class DebtCounsellingMigrationHelper {
        
        public static Type[] GetTypes() {
            return new Type[] {
                    typeof(MigrationDebtCounselling_DAO),
                    typeof(MigrationDebtCounsellingProposalItem_DAO),
                    typeof(MigrationDebtCounsellingExternalRole_DAO),
                    typeof(MigrationDebtCounsellingInternalRole_DAO),
                    typeof(MigrationDebtCounsellingProposal_DAO)};
        }
    }
}
