using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Employment_DAO is instantiated in order to create an Employment record for a Legal Entity. It is discriminated based on the
    /// Employment Type.
    /// </summary>
    /// <seealso cref="EmploymentSalaried_DAO"/>
    /// <seealso cref="EmploymentSelfEmployed_DAO"/>
    /// <seealso cref="EmploymentSubsidised_DAO"/>
    /// <seealso cref="EmploymentUnemployed_DAO"/>
    /// <seealso cref="EmploymentUnknown_DAO"/>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Employment", Schema = "dbo", DiscriminatorColumn = "EmploymentTypeKey", DiscriminatorType = "Int32", DiscriminatorValue = "0", Lazy = true)]
    public partial class Employment_DAO : DB_2AM<Employment_DAO>
    {
        private System.DateTime? _employmentStartDate;

        private System.DateTime? _employmentEndDate;

        private EmploymentType_DAO _employmentType;

        private string _contactPerson;

        private string _contactPhoneNumber;

        private string _contactPhoneCode;

        private string _confirmedBy;

        private System.DateTime? _confirmedDate;

        private string _userID;

        private System.DateTime? _changeDate;

        private string _department;

        private double? _basicIncome;

        private double? _commission;

        private double? _overtime;

        private double? _shift;

        private double? _performance;

        private double? _allowances;

        private double? _pAYE;

        private double? _uIF;

        private double? _pensionProvident;

        private double? _medicalAid;

        private double? _confirmedBasicIncome;

        private double? _confirmedCommission;

        private double? _confirmedOvertime;

        private double? _confirmedShift;

        private double? _confirmedPerformance;

        private double? _confirmedAllowances;

        private double? _confirmedPAYE;

        private double? _confirmedUIF;

        private double? _confirmedPensionProvident;

        private double? _confirmedMedicalAid;

        private double _monthlyIncome;

        private double _confirmedIncome;

        private int _Key;

        private Employer_DAO _employer;

        private EmploymentStatus_DAO _employmentStatus;

        private LegalEntity_DAO _legalEntity;

        private RemunerationType_DAO _remunerationType;

        private bool? _confirmedEmploymentFlag;

        private bool? _confirmedIncomeFlag;

        private System.Int32? _salaryPaymentDay;

        private EmploymentConfirmationSource_DAO _employmentConfirmationSource;

        private IList<EmploymentVerificationProcess_DAO> _employmentVerificationProcesses;

        private bool? unionMember;

        
        /// <summary>
        /// The date on which the Legal Entity was employed with the employer
        /// </summary>
        [Property("EmploymentStartDate", ColumnType = "Timestamp")]
        [ValidateNonEmpty("Employment Start Date is a mandatory field")]
        public virtual System.DateTime? EmploymentStartDate
        {
            get
            {
                return this._employmentStartDate;
            }
            set
            {
                this._employmentStartDate = value;
            }
        }

        /// <summary>
        /// The date on which the employment was ended.
        /// </summary>
        [Property("EmploymentEndDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? EmploymentEndDate
        {
            get
            {
                return this._employmentEndDate;
            }
            set
            {
                this._employmentEndDate = value;
            }
        }

        /// <summary>
        /// A Reference for the Legal Entity's Employment.
        /// </summary>
        [Property("ContactPerson", ColumnType = "String")]
        public virtual string ContactPerson
        {
            get
            {
                return this._contactPerson;
            }
            set
            {
                this._contactPerson = value;
            }
        }

        /// <summary>
        /// The Phone Number for the Contact Person.
        /// </summary>
        [Property("ContactPhoneNumber", ColumnType = "String", Length = 25)]
        public virtual string ContactPhoneNumber
        {
            get
            {
                return this._contactPhoneNumber;
            }
            set
            {
                this._contactPhoneNumber = value;
            }
        }

        /// <summary>
        /// The Area Code for the Contact Person's Phone Number.
        /// </summary>
        [Property("ContactPhoneCode", ColumnType = "String")]
        public virtual string ContactPhoneCode
        {
            get
            {
                return this._contactPhoneCode;
            }
            set
            {
                this._contactPhoneCode = value;
            }
        }

        /// <summary>
        /// The SAHL Employee who confirmed the Legal Entity's Income.
        /// </summary>
        [Property("ConfirmedBy", ColumnType = "String")]
        public virtual string ConfirmedBy
        {
            get
            {
                return this._confirmedBy;
            }
            set
            {
                this._confirmedBy = value;
            }
        }

        /// <summary>
        /// The date on which the Legal Entity's Income was confirmed.
        /// </summary>
        [Property("ConfirmedDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ConfirmedDate
        {
            get
            {
                return this._confirmedDate;
            }
            set
            {
                this._confirmedDate = value;
            }
        }

        /// <summary>
        /// The UserID of the user who last updated the Employment Record.
        /// </summary>
        [Property("UserID", ColumnType = "String", NotNull = false)]
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

        /// <summary>
        /// The date on which the Employment record was last changed.
        /// </summary>
        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = false)]
        public virtual System.DateTime? ChangeDate
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

        /// <summary>
        /// The department in which the Legal Entity works.
        /// </summary>
        [Property("Department", ColumnType = "String")]
        public virtual string Department
        {
            get
            {
                return this._department;
            }
            set
            {
                this._department = value;
            }
        }

        /// <summary>
        /// The Monthly Basic Income which the Legal Entity earns.
        /// </summary>
        [Property("BasicIncome", ColumnType = "Double")]
        [Lurker]
        public virtual double? BasicIncome
        {
            get
            {
                return this._basicIncome;
            }
            set
            {
                this._basicIncome = value;
            }
        }

        /// <summary>
        /// The Total Monthly Commission which the Legal Entity earns.
        /// </summary>
        [Lurker]
        [Property("Commission", ColumnType = "Double")]
        public virtual double? Commission
        {
            get
            {
                return this._commission;
            }
            set
            {
                this._commission = value;
            }
        }

        /// <summary>
        /// The Total Monthly Overtime Income which the Legal Entity earns.
        /// </summary>
        [Lurker]
        [Property("Overtime", ColumnType = "Double")]
        public virtual double? Overtime
        {
            get
            {
                return this._overtime;
            }
            set
            {
                this._overtime = value;
            }
        }

        /// <summary>
        /// The Total Monthly Shift Income which the Legal Entity earns.
        /// </summary>
        [Lurker]
        [Property("Shift", ColumnType = "Double")]
        public virtual double? Shift
        {
            get
            {
                return this._shift;
            }
            set
            {
                this._shift = value;
            }
        }

        /// <summary>
        /// The Total Monthly Performance Income which the Legal Entity earns.
        /// </summary>
        [Lurker]
        [Property("Performance", ColumnType = "Double")]
        public virtual double? Performance
        {
            get
            {
                return this._performance;
            }
            set
            {
                this._performance = value;
            }
        }

        /// <summary>
        /// The Total Monthly Allowances which the Legal Entity receives.
        /// </summary>
        [Lurker]
        [Property("Allowances", ColumnType = "Double")]
        public virtual double? Allowances
        {
            get
            {
                return this._allowances;
            }
            set
            {
                this._allowances = value;
            }
        }

        /// <summary>
        /// The Monthly PAYE paid by the Legal Entity.
        /// </summary>
        [Lurker]
        [Property("PAYE", ColumnType = "Double")]
        public virtual double? PAYE
        {
            get
            {
                return this._pAYE;
            }
            set
            {
                this._pAYE = value;
            }
        }

        /// <summary>
        /// The Monthly UIF Contribution made by the Legal Entity.
        /// </summary>
        [Lurker]
        [Property("UIF", ColumnType = "Double")]
        public virtual double? UIF
        {
            get
            {
                return this._uIF;
            }
            set
            {
                this._uIF = value;
            }
        }

        /// <summary>
        /// The monthly Pension/Provident contribution made by the Legal Entity.
        /// </summary>
        [Lurker]
        [Property("PensionProvident", ColumnType = "Double")]
        public virtual double? PensionProvident
        {
            get
            {
                return this._pensionProvident;
            }
            set
            {
                this._pensionProvident = value;
            }
        }

        /// <summary>
        /// The Monthly Medical Aid contribution made by the Legal Entity.
        /// </summary>
        [Lurker]
        [Property("MedicalAid", ColumnType = "Double")]
        public virtual double? MedicalAid
        {
            get
            {
                return this._medicalAid;
            }
            set
            {
                this._medicalAid = value;
            }
        }

        /// <summary>
        /// The Confirmed Monthly Basic Income which the Legal Entity earns.
        /// </summary>
        [Lurker]
        [Property("ConfirmedBasicIncome", ColumnType = "Double")]
        public virtual double? ConfirmedBasicIncome
        {
            get
            {
                return this._confirmedBasicIncome;
            }
            set
            {
                this._confirmedBasicIncome = value;
            }
        }

        /// <summary>
        /// The Confirmed Total Monthly Commission which the Legal Entity earns.
        /// </summary>
        [Lurker]
        [Property("ConfirmedCommission", ColumnType = "Double")]
        public virtual double? ConfirmedCommission
        {
            get
            {
                return this._confirmedCommission;
            }
            set
            {
                this._confirmedCommission = value;
            }
        }

        /// <summary>
        /// The Confirmed Total Monthly Overtime Income which the Legal Entity earns.
        /// </summary>
        [Lurker]
        [Property("ConfirmedOvertime", ColumnType = "Double")]
        public virtual double? ConfirmedOvertime
        {
            get
            {
                return this._confirmedOvertime;
            }
            set
            {
                this._confirmedOvertime = value;
            }
        }

        /// <summary>
        /// The Confirmed Total Monthly Shift Income which the Legal Entity earns.
        /// </summary>
        [Lurker]
        [Property("ConfirmedShift", ColumnType = "Double")]
        public virtual double? ConfirmedShift
        {
            get
            {
                return this._confirmedShift;
            }
            set
            {
                this._confirmedShift = value;
            }
        }

        /// <summary>
        /// The Confirmed Total Monthly Performance Income which the Legal Entity earns.
        /// </summary>
        [Lurker]
        [Property("ConfirmedPerformance", ColumnType = "Double")]
        public virtual double? ConfirmedPerformance
        {
            get
            {
                return this._confirmedPerformance;
            }
            set
            {
                this._confirmedPerformance = value;
            }
        }

        /// <summary>
        /// The Confirmed Total Monthly Allowances which the Legal Entity receives.
        /// </summary>
        [Lurker]
        [Property("ConfirmedAllowances", ColumnType = "Double")]
        public virtual double? ConfirmedAllowances
        {
            get
            {
                return this._confirmedAllowances;
            }
            set
            {
                this._confirmedAllowances = value;
            }
        }

        /// <summary>
        /// The Confirmed Monthly PAYE paid by the Legal Entity.
        /// </summary>
        [Lurker]
        [Property("ConfirmedPAYE", ColumnType = "Double")]
        public virtual double? ConfirmedPAYE
        {
            get
            {
                return this._confirmedPAYE;
            }
            set
            {
                this._confirmedPAYE = value;
            }
        }

        /// <summary>
        /// The Confirmed Monthly UIF Contribution made by the Legal Entity.
        /// </summary>
        [Lurker]
        [Property("ConfirmedUIF", ColumnType = "Double")]
        public virtual double? ConfirmedUIF
        {
            get
            {
                return this._confirmedUIF;
            }
            set
            {
                this._confirmedUIF = value;
            }
        }

        /// <summary>
        /// The Confirmed Monthly Pension/Provident contribution made by the Legal Entity.
        /// </summary>
        [Lurker]
        [Property("ConfirmedPensionProvident", ColumnType = "Double")]
        public virtual double? ConfirmedPensionProvident
        {
            get
            {
                return this._confirmedPensionProvident;
            }
            set
            {
                this._confirmedPensionProvident = value;
            }
        }

        /// <summary>
        /// The Confirmed Monthly Medical Aid contribution made by the Legal Entity.
        /// </summary>
        [Lurker]
        [Property("ConfirmedMedicalAid", ColumnType = "Double")]
        public virtual double? ConfirmedMedicalAid
        {
            get
            {
                return this._confirmedMedicalAid;
            }
            set
            {
                this._confirmedMedicalAid = value;
            }
        }

        /// <summary>
        /// The Confirmed Monthly Income of the Legal Entity.
        /// </summary>
        [Lurker]
        [Property("MonthlyIncome", ColumnType = "Double", Insert = false, Update = false)]
        public virtual double MonthlyIncome
        {
            get
            {
                return this._monthlyIncome;
            }
            set
            {
                this._monthlyIncome = value;
            }
        }

        /// <summary>
        /// The Confirmed Monthly Income of the Legal Entity.
        /// </summary>
        [Lurker]
        [Property("ConfirmedIncome", ColumnType = "Double", Insert = false, Update = false)]
        public virtual double ConfirmedIncome
        {
            get
            {
                return this._confirmedIncome;
            }
            set
            {
                this._confirmedIncome = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "EmploymentKey", ColumnType = "Int32")]
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

        /// <summary>
        /// Each Employment record belongs to a single Employer. This is the foreign key reference to the Employer table.
        /// </summary>
        [BelongsTo("EmployerKey", NotNull = false)]
        public virtual Employer_DAO Employer
        {
            get
            {
                return this._employer;
            }
            set
            {
                this._employer = value;
            }
        }

        /// <summary>
        /// Each Employment record is assigned an EmploymentStatus. This is the foreign key reference to the EmploymentStatus table.
        /// </summary>
        [Lurker]
        [BelongsTo("EmploymentStatusKey", NotNull = true)]
        [ValidateNonEmpty("Employment Status is a mandatory field")]
        public virtual EmploymentStatus_DAO EmploymentStatus
        {
            get
            {
                return this._employmentStatus;
            }
            set
            {
                this._employmentStatus = value;
            }
        }

        /// <summary>
        /// Specifies the type of Employment e.g. Salaried, SelfEmployed, etc.
        /// </summary>
        /// <remarks>This is currently not exposed in business model and is used only in queries.</remarks>
        [Lurker]
        [BelongsTo("EmploymentTypeKey", NotNull = true, Access = PropertyAccess.FieldCamelcaseUnderscore, Insert = false, Update = false)]
        public virtual EmploymentType_DAO EmploymentType
        {
            get
            {
                return this._employmentType;
            }
        }

        /// <summary>
        /// Each Employment record belongs to a LegalEntity. This is the foreign key reference to the LegalEntity table.
        /// </summary>
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

        /// <summary>
        /// Each Employment record has its own remuneration type. This is the foreign reference to RumunerationType table. e.g. Salaried/Commission Only
        /// </summary>
        [BelongsTo("RemunerationTypeKey", NotNull = true)]
        [ValidateNonEmpty("Remuneration Type is a mandatory field")]
        [Lurker]
        public virtual RemunerationType_DAO RemunerationType
        {
            get
            {
                return this._remunerationType;
            }
            set
            {
                this._remunerationType = value;
            }
        }

        [Property("ConfirmedEmploymentFlag", ColumnType = "Boolean")]
        public virtual bool? ConfirmedEmploymentFlag
        {
            get { return this._confirmedEmploymentFlag; }
            set { this._confirmedEmploymentFlag = value; }
        }

        [Property("ConfirmedIncomeFlag", ColumnType = "Boolean")]
        public virtual bool? ConfirmedIncomeFlag
        {
            get { return this._confirmedIncomeFlag; }
            set { this._confirmedIncomeFlag = value; }
        }

        [Property("SalaryPaymentDay", ColumnType = "Int32")]
        public virtual System.Int32? SalaryPaymentDay
        {
            get { return this._salaryPaymentDay; }
            set { this._salaryPaymentDay = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Property("UnionMember", ColumnType = "Boolean")]
        public virtual bool? UnionMember
        {
            get
            {
                return this.unionMember;
            }
            set
            {
                this.unionMember = value;
            }
        }


        [BelongsTo("EmploymentConfirmationSourceKey", NotNull = false)]
        [Lurker]
        public virtual EmploymentConfirmationSource_DAO EmploymentConfirmationSource
        {
            get { return this._employmentConfirmationSource; }
            set { this._employmentConfirmationSource = value; }
        }

        [HasMany(typeof(EmploymentVerificationProcess_DAO), ColumnKey = "EmploymentKey", Table = "EmploymentVerificationProcess", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<EmploymentVerificationProcess_DAO> EmploymentVerificationProcesses
        {
            get { return this._employmentVerificationProcesses; }
            set { this._employmentVerificationProcesses = value; }
        }
    }
}