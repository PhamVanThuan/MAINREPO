using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [DoNotTestWithGenericTest]
    [ActiveRecord("Regent", Schema = "dbo")]
    public partial class Regent_DAO : DB_2AM<Regent_DAO>
    {
        private string _regentClientSalutation;

        private string _regentClientSurname;

        private string _regentClientFirstNames;

        private decimal _regentClientIDNumber;

        private short _regentClientGender;

        private System.DateTime? _regentClientDateBirth;

        private RegentStatus_DAO _regentStatus;

        private System.DateTime? _regentApplicationDate;

        private System.DateTime? _regentInceptionDate;

        private System.DateTime? _regentExpiryDate;

        private decimal _regentLoanTerm;

        private double _regentSumInsured;

        private double _regentPremium;

        private string _regentClientSecSalutation;

        private string _regentClientSecSurname;

        private string _regentClientSecFirstNames;

        private decimal _regentClientSecIDNumber;

        private short _regentClientSecGender;

        private System.DateTime? _regentClientSecDateBirth;

        private short _regentJointIndicator;

        private System.DateTime? _regentReinstateDate;

        private System.DateTime? _regentLastUpdateDate;

        private double _regentCommision;

        private int _regentUnderwritingFirst;

        private int _regentUnderwritingSecond;

        private decimal _sAHLEmployeeNumber;

        private short _regentUpdatedStatus;

        private short _regentClientOccupation;

        private short _regentClientAge;

        private string _replacementPolicy;

        private string _adviceRequired;

        private string _lifeAssuredName;

        private string _oldInsurer;

        private string _oldPolicyNo;

        private System.DateTime? _regentNewBusinessDate;

        private double _capitalizedMonthlyBalance;

        private decimal _key;

        [Property("RegentClientSalutation", ColumnType = "String")]
        public virtual string RegentClientSalutation
        {
            get
            {
                return this._regentClientSalutation;
            }
            set
            {
                this._regentClientSalutation = value;
            }
        }

        [Property("RegentClientSurname", ColumnType = "String")]
        public virtual string RegentClientSurname
        {
            get
            {
                return this._regentClientSurname;
            }
            set
            {
                this._regentClientSurname = value;
            }
        }

        [Property("RegentClientFirstNames", ColumnType = "String")]
        public virtual string RegentClientFirstNames
        {
            get
            {
                return this._regentClientFirstNames;
            }
            set
            {
                this._regentClientFirstNames = value;
            }
        }

        [Property("RegentClientIDNumber", ColumnType = "Decimal")]
        public virtual decimal RegentClientIDNumber
        {
            get
            {
                return this._regentClientIDNumber;
            }
            set
            {
                this._regentClientIDNumber = value;
            }
        }

        [Property("RegentClientGender", ColumnType = "Int16")]
        public virtual short RegentClientGender
        {
            get
            {
                return this._regentClientGender;
            }
            set
            {
                this._regentClientGender = value;
            }
        }

        [Property("RegentClientDateBirth", ColumnType = "Timestamp")]
        public virtual System.DateTime? RegentClientDateBirth
        {
            get
            {
                return this._regentClientDateBirth;
            }
            set
            {
                this._regentClientDateBirth = value;
            }
        }

        [BelongsTo("RegentPolicyStatus")]
        public virtual RegentStatus_DAO RegentStatus
        {
            get
            {
                return this._regentStatus;
            }
            set
            {
                this._regentStatus = value;
            }
        }

        [Property("RegentApplicationDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? RegentApplicationDate
        {
            get
            {
                return this._regentApplicationDate;
            }
            set
            {
                this._regentApplicationDate = value;
            }
        }

        [Property("RegentInceptionDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? RegentInceptionDate
        {
            get
            {
                return this._regentInceptionDate;
            }
            set
            {
                this._regentInceptionDate = value;
            }
        }

        [Property("RegentExpiryDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? RegentExpiryDate
        {
            get
            {
                return this._regentExpiryDate;
            }
            set
            {
                this._regentExpiryDate = value;
            }
        }

        [Property("RegentLoanTerm", ColumnType = "Decimal")]
        public virtual decimal RegentLoanTerm
        {
            get
            {
                return this._regentLoanTerm;
            }
            set
            {
                this._regentLoanTerm = value;
            }
        }

        [Property("RegentSumInsured", ColumnType = "Double")]
        public virtual double RegentSumInsured
        {
            get
            {
                return this._regentSumInsured;
            }
            set
            {
                this._regentSumInsured = value;
            }
        }

        [Property("RegentPremium", ColumnType = "Double")]
        public virtual double RegentPremium
        {
            get
            {
                return this._regentPremium;
            }
            set
            {
                this._regentPremium = value;
            }
        }

        [Property("RegentClientSecSalutation", ColumnType = "String")]
        public virtual string RegentClientSecSalutation
        {
            get
            {
                return this._regentClientSecSalutation;
            }
            set
            {
                this._regentClientSecSalutation = value;
            }
        }

        [Property("RegentClientSecSurname", ColumnType = "String")]
        public virtual string RegentClientSecSurname
        {
            get
            {
                return this._regentClientSecSurname;
            }
            set
            {
                this._regentClientSecSurname = value;
            }
        }

        [Property("RegentClientSecFirstNames", ColumnType = "String")]
        public virtual string RegentClientSecFirstNames
        {
            get
            {
                return this._regentClientSecFirstNames;
            }
            set
            {
                this._regentClientSecFirstNames = value;
            }
        }

        [Property("RegentClientSecIDNumber", ColumnType = "Decimal")]
        public virtual decimal RegentClientSecIDNumber
        {
            get
            {
                return this._regentClientSecIDNumber;
            }
            set
            {
                this._regentClientSecIDNumber = value;
            }
        }

        [Property("RegentClientSecGender", ColumnType = "Int16")]
        public virtual short RegentClientSecGender
        {
            get
            {
                return this._regentClientSecGender;
            }
            set
            {
                this._regentClientSecGender = value;
            }
        }

        [Property("RegentClientSecDateBirth", ColumnType = "Timestamp")]
        public virtual System.DateTime? RegentClientSecDateBirth
        {
            get
            {
                return this._regentClientSecDateBirth;
            }
            set
            {
                this._regentClientSecDateBirth = value;
            }
        }

        [Property("RegentJointIndicator", ColumnType = "Int16")]
        public virtual short RegentJointIndicator
        {
            get
            {
                return this._regentJointIndicator;
            }
            set
            {
                this._regentJointIndicator = value;
            }
        }

        [Property("RegentReinstateDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? RegentReinstateDate
        {
            get
            {
                return this._regentReinstateDate;
            }
            set
            {
                this._regentReinstateDate = value;
            }
        }

        [Property("RegentLastUpdateDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? RegentLastUpdateDate
        {
            get
            {
                return this._regentLastUpdateDate;
            }
            set
            {
                this._regentLastUpdateDate = value;
            }
        }

        [Property("RegentCommision", ColumnType = "Double")]
        public virtual double RegentCommision
        {
            get
            {
                return this._regentCommision;
            }
            set
            {
                this._regentCommision = value;
            }
        }

        [Property("RegentUnderwritingFirst", ColumnType = "Int32")]
        public virtual int RegentUnderwritingFirst
        {
            get
            {
                return this._regentUnderwritingFirst;
            }
            set
            {
                this._regentUnderwritingFirst = value;
            }
        }

        [Property("RegentUnderwritingSecond", ColumnType = "Int32")]
        public virtual int RegentUnderwritingSecond
        {
            get
            {
                return this._regentUnderwritingSecond;
            }
            set
            {
                this._regentUnderwritingSecond = value;
            }
        }

        [Property("SAHLEmployeeNumber", ColumnType = "Decimal")]
        public virtual decimal SAHLEmployeeNumber
        {
            get
            {
                return this._sAHLEmployeeNumber;
            }
            set
            {
                this._sAHLEmployeeNumber = value;
            }
        }

        [Property("RegentUpdatedStatus", ColumnType = "Int16")]
        public virtual short RegentUpdatedStatus
        {
            get
            {
                return this._regentUpdatedStatus;
            }
            set
            {
                this._regentUpdatedStatus = value;
            }
        }

        [Property("RegentClientOccupation", ColumnType = "Int16")]
        public virtual short RegentClientOccupation
        {
            get
            {
                return this._regentClientOccupation;
            }
            set
            {
                this._regentClientOccupation = value;
            }
        }

        [Property("RegentClientAge", ColumnType = "Int16")]
        public virtual short RegentClientAge
        {
            get
            {
                return this._regentClientAge;
            }
            set
            {
                this._regentClientAge = value;
            }
        }

        [Property("ReplacementPolicy", ColumnType = "String", Length = 3)]
        public virtual string ReplacementPolicy
        {
            get
            {
                return this._replacementPolicy;
            }
            set
            {
                this._replacementPolicy = value;
            }
        }

        [Property("AdviceRequired", ColumnType = "String", Length = 3)]
        public virtual string AdviceRequired
        {
            get
            {
                return this._adviceRequired;
            }
            set
            {
                this._adviceRequired = value;
            }
        }

        [Property("LifeAssuredName", ColumnType = "String")]
        public virtual string LifeAssuredName
        {
            get
            {
                return this._lifeAssuredName;
            }
            set
            {
                this._lifeAssuredName = value;
            }
        }

        [Property("OldInsurer", ColumnType = "String")]
        public virtual string OldInsurer
        {
            get
            {
                return this._oldInsurer;
            }
            set
            {
                this._oldInsurer = value;
            }
        }

        [Property("OldPolicyNo", ColumnType = "String")]
        public virtual string OldPolicyNo
        {
            get
            {
                return this._oldPolicyNo;
            }
            set
            {
                this._oldPolicyNo = value;
            }
        }

        [Property("RegentNewBusinessDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? RegentNewBusinessDate
        {
            get
            {
                return this._regentNewBusinessDate;
            }
            set
            {
                this._regentNewBusinessDate = value;
            }
        }

        [Property("CapitalizedMonthlyBalance", ColumnType = "Double")]
        public virtual double CapitalizedMonthlyBalance
        {
            get
            {
                return this._capitalizedMonthlyBalance;
            }
            set
            {
                this._capitalizedMonthlyBalance = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "LoanNumber", ColumnType = "Decimal")]
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