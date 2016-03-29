using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ConstructorInjector]
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("HOC", Schema = "dbo", Lazy = true)]
    public partial class HOC_DAO : DB_2AM<HOC_DAO>
    {
        private string _hOCPolicyNumber;

        private double _hOCProrataPremium;

        private double? _hOCMonthlyPremium;

        private double? _hOCThatchAmount;

        private double? _hOCConventionalAmount;

        private double? _hOCShingleAmount;

        private double _hOCTotalSumInsured;

        private int? _hOCStatusID;

        private bool? _hOCSBICFlag;

        private double? _capitalizedMonthlyBalance;

        private System.DateTime? _commencementDate;

        private System.DateTime? _anniversaryDate;

        private string _userID;

        private System.DateTime? _changeDate;

        private bool _ceded;

        private string _sAHLPolicyNumber;

        private System.DateTime? _cancellationDate;

        private HOCHistory_DAO _hOCHistory;

        private IList<HOCHistory_DAO> _hOCHistories;

        private HOCInsurer_DAO _hOCInsurer;

        // we have derived from this class.
        //private FinancialService_DAO _financialService;

        private HOCConstruction_DAO _hOCConstruction;

        private HOCRoof_DAO _hOCRoof;

        private HOCStatus_DAO _hOCStatus;

        private HOCSubsidence_DAO _hOCSubsidence;

        private double _hOCAdministrationFee;

        private double _hOCBasePremium;

        private double _sASRIAAmount;

        private FinancialService_DAO _financialService;

        private int _key;

        /// <summary>
        /// Used for Activerecord exclusively, please use Key.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Foreign, Column = "FinancialServiceKey")]
        public virtual int Key
        {
            get { return this._key; }
            set { this._key = value; }
        }

        [Property("HOCPolicyNumber", ColumnType = "String", Length = 25)]
        public virtual string HOCPolicyNumber
        {
            get
            {
                return this._hOCPolicyNumber;
            }
            set
            {
                this._hOCPolicyNumber = value;
            }
        }

        [Property("HOCProrataPremium", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("HOC Prorata Premium is a mandatory field")]
        public virtual double HOCProrataPremium
        {
            get
            {
                return this._hOCProrataPremium;
            }
            set
            {
                this._hOCProrataPremium = value;
            }
        }

        [Property("HOCMonthlyPremium", ColumnType = "Double")]
        public virtual double? HOCMonthlyPremium
        {
            get
            {
                return this._hOCMonthlyPremium;
            }
            set
            {
                this._hOCMonthlyPremium = value;
            }
        }

        [Property("HOCThatchAmount", ColumnType = "Double")]
        public virtual double? HOCThatchAmount
        {
            get
            {
                return this._hOCThatchAmount;
            }
            set
            {
                this._hOCThatchAmount = value;
            }
        }

        [Property("HOCConventionalAmount", ColumnType = "Double")]
        public virtual double? HOCConventionalAmount
        {
            get
            {
                return this._hOCConventionalAmount;
            }
            set
            {
                this._hOCConventionalAmount = value;
            }
        }

        [Property("HOCShingleAmount", ColumnType = "Double")]
        public virtual double? HOCShingleAmount
        {
            get
            {
                return this._hOCShingleAmount;
            }
            set
            {
                this._hOCShingleAmount = value;
            }
        }

        [Lurker]
        [Property("HOCTotalSumInsured", ColumnType = "Double")]
        public virtual double HOCTotalSumInsured
        {
            get
            {
                return this._hOCTotalSumInsured;
            }
            set
            {
                this._hOCTotalSumInsured = value;
            }
        }

        [Property("HOCStatusID", ColumnType = "Int32")]
        public virtual int? HOCStatusID
        {
            get
            {
                return this._hOCStatusID;
            }
            set
            {
                this._hOCStatusID = value;
            }
        }

        [Property("HOCSBICFlag", ColumnType = "Boolean")]
        public virtual bool? HOCSBICFlag
        {
            get
            {
                return this._hOCSBICFlag;
            }
            set
            {
                this._hOCSBICFlag = value;
            }
        }

        [Property("CapitalizedMonthlyBalance", ColumnType = "Double")]
        public virtual double? CapitalizedMonthlyBalance
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

        [Property("CommencementDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? CommencementDate
        {
            get
            {
                return this._commencementDate;
            }
            set
            {
                this._commencementDate = value;
            }
        }

        [Lurker]
        [Property("AnniversaryDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? AnniversaryDate
        {
            get
            {
                return this._anniversaryDate;
            }
            set
            {
                this._anniversaryDate = value;
            }
        }

        [Property("UserID", ColumnType = "String")]
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

        [Property("ChangeDate", ColumnType = "Timestamp")]
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

        [Property("Ceded", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Ceded is a mandatory field")]
        public virtual bool Ceded
        {
            get
            {
                return this._ceded;
            }
            set
            {
                this._ceded = value;
            }
        }

        [Property("SAHLPolicyNumber", ColumnType = "String")]
        public virtual string SAHLPolicyNumber
        {
            get
            {
                return this._sAHLPolicyNumber;
            }
            set
            {
                this._sAHLPolicyNumber = value;
            }
        }

        [Property("CancellationDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? CancellationDate
        {
            get
            {
                return this._cancellationDate;
            }
            set
            {
                this._cancellationDate = value;
            }
        }

        [BelongsTo("HOCHistoryKey")]
        public virtual HOCHistory_DAO HOCHistory
        {
            get
            {
                return this._hOCHistory;
            }
            set
            {
                this._hOCHistory = value;
            }
        }

        [HasMany(typeof(HOCHistory_DAO), ColumnKey = "FinancialServiceKey", Table = "HOCHistory", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<HOCHistory_DAO> HOCHistories
        {
            get
            {
                return this._hOCHistories;
            }
            set
            {
                this._hOCHistories = value;
            }
        }

        [BelongsTo("HOCInsurerKey", NotNull = false)]
        public virtual HOCInsurer_DAO HOCInsurer
        {
            get
            {
                return this._hOCInsurer;
            }
            set
            {
                this._hOCInsurer = value;
            }
        }

        [BelongsTo("HOCConstructionKey", NotNull = false)]
        public virtual HOCConstruction_DAO HOCConstruction
        {
            get
            {
                return this._hOCConstruction;
            }
            set
            {
                this._hOCConstruction = value;
            }
        }

        [BelongsTo("HOCRoofKey", NotNull = true)]
        [ValidateNonEmpty("HOC Roof is a mandatory field")]
        public virtual HOCRoof_DAO HOCRoof
        {
            get
            {
                return this._hOCRoof;
            }
            set
            {
                this._hOCRoof = value;
            }
        }

        [BelongsTo("HOCStatusKey", NotNull = true)]
        [ValidateNonEmpty("HOC Status is a mandatory field")]
        public virtual HOCStatus_DAO HOCStatus
        {
            get
            {
                return this._hOCStatus;
            }
            set
            {
                this._hOCStatus = value;
            }
        }

        [BelongsTo("HOCSubsidenceKey", NotNull = false)]
        public virtual HOCSubsidence_DAO HOCSubsidence
        {
            get
            {
                return this._hOCSubsidence;
            }
            set
            {
                this._hOCSubsidence = value;
            }
        }

        [Property("HOCAdministrationFee", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("HOC Administration Fee is a mandatory field")]
        public virtual double HOCAdministrationFee
        {
            get
            {
                return this._hOCAdministrationFee;
            }
            set
            {
                this._hOCAdministrationFee = value;
            }
        }

        [Property("HOCBasePremium", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("HOC Base Premium is a mandatory field")]
        public virtual double HOCBasePremium
        {
            get
            {
                return this._hOCBasePremium;
            }
            set
            {
                this._hOCBasePremium = value;
            }
        }

        [Property("SASRIAAmount", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("SASRIA Amount is a mandatory field")]
        public virtual double SASRIAAmount
        {
            get
            {
                return this._sASRIAAmount;
            }
            set
            {
                this._sASRIAAmount = value;
            }
        }

        [OneToOne]
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
    }
}