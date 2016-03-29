using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using System;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("DisabilityClaim", Schema = "dbo", Lazy = true)]
    [ConstructorInjector]
    public partial class DisabilityClaim_DAO : DB_2AM<DisabilityClaim_DAO>
    {
        private int _key;

        private Account_DAO _account;

        private LegalEntity_DAO _legalEntity;

        private DateTime _dateClaimReceived;

        private DateTime? _lastDateWorked;

        private DateTime? _dateOfDiagnosis;

        private string _claimantOccupation;

        private DisabilityType_DAO _disabilityType;

        private string _otherDisabilityComments;

        private DateTime? _expectedReturnToWorkDate;

        private DisabilityClaimStatus_DAO _disabilityClaimStatus;

        private DateTime? _paymentStartDate;

        private int? _numberOfInstalmentsAuthorised;

        private DateTime? _paymentEndDate;

        /// <summary>
        /// Used for Activerecord exclusively, please use Key.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "DisabilityClaimKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get { return _key; }
            set { _key = value; }
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

        [BelongsTo("LegalEntityKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity is a mandatory field")]
        public virtual LegalEntity_DAO LegalEntity
        {
            get
            {
                return this._legalEntity;
            }
            set
            {
                this._legalEntity = value;
            }
        }

        [Property("DateClaimReceived", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Date Claim Received is a mandatory field")]
        public virtual System.DateTime DateClaimReceived
        {
            get
            {
                return this._dateClaimReceived;
            }
            set
            {
                this._dateClaimReceived = value;
            }
        }

        [Property("LastDateWorked", ColumnType = "Timestamp", NotNull = false)]
        public virtual System.DateTime? LastDateWorked
        {
            get
            {
                return this._lastDateWorked;
            }
            set
            {
                this._lastDateWorked = value;
            }
        }

        [Property("DateOfDiagnosis", ColumnType = "Timestamp", NotNull = false)]
        public virtual System.DateTime? DateOfDiagnosis
        {
            get
            {
                return this._dateOfDiagnosis;
            }
            set
            {
                this._dateOfDiagnosis = value;
            }
        }

        [Property("ClaimantOccupation", ColumnType = "String", Length = 255, NotNull = false)]
        public virtual string ClaimantOccupation
        {
            get
            {
                return this._claimantOccupation;
            }
            set
            {
                this._claimantOccupation = value;
            }
        }

        [BelongsTo("DisabilityTypeKey", NotNull = false)]
        public virtual DisabilityType_DAO DisabilityType
        {
            get
            {
                return this._disabilityType;
            }
            set
            {
                this._disabilityType = value;
            }
        }

        [Property("OtherDisabilityComments", ColumnType = "String", Length = 8000, NotNull = false)]
        public virtual string OtherDisabilityComments
        {
            get
            {
                return this._otherDisabilityComments;
            }
            set
            {
                this._otherDisabilityComments = value;
            }
        }

        [Property("ExpectedReturnToWorkDate", ColumnType = "Timestamp", NotNull = false)]
        public virtual System.DateTime? ExpectedReturnToWorkDate
        {
            get
            {
                return this._expectedReturnToWorkDate;
            }
            set
            {
                this._expectedReturnToWorkDate = value;
            }
        }

        [BelongsTo("DisabilityClaimStatusKey", NotNull = true)]
        [ValidateNonEmpty("Disability Claim Status is a mandatory field")]
        public virtual DisabilityClaimStatus_DAO DisabilityClaimStatus
        {
            get
            {
                return this._disabilityClaimStatus;
            }
            set
            {
                this._disabilityClaimStatus = value;
            }
        }

        [Property("PaymentStartDate", ColumnType = "Timestamp", NotNull = false)]
        public virtual System.DateTime? PaymentStartDate
        {
            get
            {
                return this._paymentStartDate;
            }
            set
            {
                this._paymentStartDate = value;
            }
        }

        [Property("NumberOfInstalmentsAuthorised", ColumnType = "Int32", NotNull = false)]
        public virtual int? NumberOfInstalmentsAuthorised
        {
            get
            {
                return this._numberOfInstalmentsAuthorised;
            }
            set
            {
                this._numberOfInstalmentsAuthorised = value;
            }
        }

        [Property("PaymentEndDate", ColumnType = "Timestamp", NotNull = false)]
        public virtual System.DateTime? PaymentEndDate
        {
            get
            {
                return this._paymentEndDate;
            }
            set
            {
                this._paymentEndDate = value;
            }
        }
    }
}