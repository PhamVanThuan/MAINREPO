using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    //[GenericTest(Globals.TestType.Find)]
    [DoNotTestWithGenericTest]
    [ActiveRecord("RegMail", Schema = "dbo")]
    public partial class RegMail_DAO : DB_SAHL<RegMail_DAO>
    {
        private int _loanNumber;

        private decimal _purposeNumber;

        private decimal _detailTypeNumber;

        private decimal _attorneyNumber;

        private short? _regMailLoanStatus;

        private System.DateTime? _regMailDateTime;

        private double? _regMailBondAmount;

        private System.DateTime? _regMailBondDate;

        private double? _regMailGuaranteeAmount;

        private double? _regMailCashRequired;

        private double? _regMailCashDeposit;

        private double? _regMailConveyancingFee;

        private double? _regMailVAT;

        private double? _regMailTransferDuty;

        private double? _regMailDeedsFee;

        private string _regMailInstructions1;

        private string _regMailInstructions2;

        private string _regMailInstructions3;

        private double? _regMailStampDuty;

        private double? _regMailCancelFee;

        private short? _regMailCATSFlag;

        private double? _regMailLoanAgreementAmount;

        private double? _regMailValuationFee;

        private double? _regMailAdminFee;

        private double? _quickCashInterest;

        private double? _qCAdminFee;

        private decimal _key;

        [Property("LoanNumber", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Loan Number is a mandatory field")]
        public virtual int LoanNumber
        {
            get
            {
                return this._loanNumber;
            }
            set
            {
                this._loanNumber = value;
            }
        }

        [Property("PurposeNumber", ColumnType = "Decimal", NotNull = true)]
        [ValidateNonEmpty("Purpose Number is a mandatory field")]
        public virtual decimal PurposeNumber
        {
            get
            {
                return this._purposeNumber;
            }
            set
            {
                this._purposeNumber = value;
            }
        }

        [Property("DetailTypeNumber", ColumnType = "Decimal", NotNull = true)]
        [ValidateNonEmpty("Detail Type Number is a mandatory field")]
        public virtual decimal DetailTypeNumber
        {
            get
            {
                return this._detailTypeNumber;
            }
            set
            {
                this._detailTypeNumber = value;
            }
        }

        [Property("AttorneyNumber", ColumnType = "Decimal", NotNull = true)]
        [ValidateNonEmpty("Attorney Number is a mandatory field")]
        public virtual decimal AttorneyNumber
        {
            get
            {
                return this._attorneyNumber;
            }
            set
            {
                this._attorneyNumber = value;
            }
        }

        [Property("RegMailLoanStatus", ColumnType = "Int16")]
        public virtual short? RegMailLoanStatus
        {
            get
            {
                return this._regMailLoanStatus;
            }
            set
            {
                this._regMailLoanStatus = value;
            }
        }

        [Property("RegMailDateTime", ColumnType = "Timestamp")]
        public virtual System.DateTime? RegMailDateTime
        {
            get
            {
                return this._regMailDateTime;
            }
            set
            {
                this._regMailDateTime = value;
            }
        }

        [Property("RegMailBondAmount", ColumnType = "Double")]
        public virtual double? RegMailBondAmount
        {
            get
            {
                return this._regMailBondAmount;
            }
            set
            {
                this._regMailBondAmount = value;
            }
        }

        [Property("RegMailBondDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? RegMailBondDate
        {
            get
            {
                return this._regMailBondDate;
            }
            set
            {
                this._regMailBondDate = value;
            }
        }

        [Property("RegMailGuaranteeAmount", ColumnType = "Double")]
        public virtual double? RegMailGuaranteeAmount
        {
            get
            {
                return this._regMailGuaranteeAmount;
            }
            set
            {
                this._regMailGuaranteeAmount = value;
            }
        }

        [Property("RegMailCashRequired", ColumnType = "Double")]
        public virtual double? RegMailCashRequired
        {
            get
            {
                return this._regMailCashRequired;
            }
            set
            {
                this._regMailCashRequired = value;
            }
        }

        [Property("RegMailCashDeposit", ColumnType = "Double")]
        public virtual double? RegMailCashDeposit
        {
            get
            {
                return this._regMailCashDeposit;
            }
            set
            {
                this._regMailCashDeposit = value;
            }
        }

        [Property("RegMailConveyancingFee", ColumnType = "Double")]
        public virtual double? RegMailConveyancingFee
        {
            get
            {
                return this._regMailConveyancingFee;
            }
            set
            {
                this._regMailConveyancingFee = value;
            }
        }

        [Property("RegMailVAT", ColumnType = "Double")]
        public virtual double? RegMailVAT
        {
            get
            {
                return this._regMailVAT;
            }
            set
            {
                this._regMailVAT = value;
            }
        }

        [Property("RegMailTransferDuty", ColumnType = "Double")]
        public virtual double? RegMailTransferDuty
        {
            get
            {
                return this._regMailTransferDuty;
            }
            set
            {
                this._regMailTransferDuty = value;
            }
        }

        [Property("RegMailDeedsFee", ColumnType = "Double")]
        public virtual double? RegMailDeedsFee
        {
            get
            {
                return this._regMailDeedsFee;
            }
            set
            {
                this._regMailDeedsFee = value;
            }
        }

        [Property("RegMailInstructions1", ColumnType = "String")]
        public virtual string RegMailInstructions1
        {
            get
            {
                return this._regMailInstructions1;
            }
            set
            {
                this._regMailInstructions1 = value;
            }
        }

        [Property("RegMailInstructions2", ColumnType = "String")]
        public virtual string RegMailInstructions2
        {
            get
            {
                return this._regMailInstructions2;
            }
            set
            {
                this._regMailInstructions2 = value;
            }
        }

        [Property("RegMailInstructions3", ColumnType = "String")]
        public virtual string RegMailInstructions3
        {
            get
            {
                return this._regMailInstructions3;
            }
            set
            {
                this._regMailInstructions3 = value;
            }
        }

        [Property("RegMailStampDuty", ColumnType = "Double")]
        public virtual double? RegMailStampDuty
        {
            get
            {
                return this._regMailStampDuty;
            }
            set
            {
                this._regMailStampDuty = value;
            }
        }

        [Property("RegMailCancelFee", ColumnType = "Double")]
        public virtual double? RegMailCancelFee
        {
            get
            {
                return this._regMailCancelFee;
            }
            set
            {
                this._regMailCancelFee = value;
            }
        }

        [Property("RegMailCATSFlag", ColumnType = "Int16")]
        public virtual short? RegMailCATSFlag
        {
            get
            {
                return this._regMailCATSFlag;
            }
            set
            {
                this._regMailCATSFlag = value;
            }
        }

        [Property("RegMailLoanAgreementAmount", ColumnType = "Double")]
        public virtual double? RegMailLoanAgreementAmount
        {
            get
            {
                return this._regMailLoanAgreementAmount;
            }
            set
            {
                this._regMailLoanAgreementAmount = value;
            }
        }

        [Property("RegMailValuationFee", ColumnType = "Double")]
        public virtual double? RegMailValuationFee
        {
            get
            {
                return this._regMailValuationFee;
            }
            set
            {
                this._regMailValuationFee = value;
            }
        }

        [Property("RegMailAdminFee", ColumnType = "Double")]
        public virtual double? RegMailAdminFee
        {
            get
            {
                return this._regMailAdminFee;
            }
            set
            {
                this._regMailAdminFee = value;
            }
        }

        [Property("QuickCashInterest", ColumnType = "Double")]
        public virtual double? QuickCashInterest
        {
            get
            {
                return this._quickCashInterest;
            }
            set
            {
                this._quickCashInterest = value;
            }
        }

        [Property("QCAdminFee", ColumnType = "Double")]
        public virtual double? QCAdminFee
        {
            get
            {
                return this._qCAdminFee;
            }
            set
            {
                this._qCAdminFee = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "RegMailNumber", ColumnType = "Decimal")]
        public virtual decimal Key
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
    }
}