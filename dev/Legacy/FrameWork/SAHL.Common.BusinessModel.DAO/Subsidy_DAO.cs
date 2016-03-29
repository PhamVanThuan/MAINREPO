using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Subsidy", Schema = "dbo", Lazy = true)]
    public partial class Subsidy_DAO : DB_2AM<Subsidy_DAO>
    {
        private string _salaryNumber;

        private string _paypoint;

        private string _notch;

        private string _rank;

        private double _stopOrderAmount;

        private int _Key;

        private EmploymentSubsidised_DAO _employment;

        private GeneralStatus_DAO _generalStatus;

        private LegalEntity_DAO _legalEntity;

        private SubsidyProvider_DAO _subsidyProvider;

        private IList<Application_DAO> _applications;

        private IList<Account_DAO> _accounts;

        private bool _gepfMember;

        [Property("SalaryNumber", ColumnType = "String", NotNull = false)]
        public virtual string SalaryNumber
        {
            get
            {
                return this._salaryNumber;
            }
            set
            {
                this._salaryNumber = value;
            }
        }

        [Property("Paypoint", ColumnType = "String")]
        public virtual string Paypoint
        {
            get
            {
                return this._paypoint;
            }
            set
            {
                this._paypoint = value;
            }
        }

        [Property("Notch", ColumnType = "String")]
        public virtual string Notch
        {
            get
            {
                return this._notch;
            }
            set
            {
                this._notch = value;
            }
        }

        [Property("Rank", ColumnType = "String")]
        public virtual string Rank
        {
            get
            {
                return this._rank;
            }
            set
            {
                this._rank = value;
            }
        }

        [Property("StopOrderAmount", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Stop Order Amount is a mandatory field")]
        public virtual double StopOrderAmount
        {
            get
            {
                return this._stopOrderAmount;
            }
            set
            {
                this._stopOrderAmount = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "SubsidyKey", ColumnType = "Int32")]
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

        [BelongsTo("EmploymentKey", NotNull = true)]
        [ValidateNonEmpty("Employment is a mandatory field")]
        public virtual EmploymentSubsidised_DAO Employment
        {
            get
            {
                return this._employment;
            }
            set
            {
                this._employment = value;
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

        [Lurker]
        [HasAndBelongsToMany(typeof(Application_DAO), Table = "OfferSubsidy", ColumnKey = "SubsidyKey", ColumnRef = "OfferKey", Lazy = true)]
        public virtual IList<Application_DAO> Applications
        {
            get { return _applications; }
            set { _applications = value; }
        }

        [Lurker]
        [HasAndBelongsToMany(typeof(Account_DAO), Table = "AccountSubsidy", ColumnKey = "SubsidyKey", ColumnRef = "AccountKey", Lazy = true)]
        public virtual IList<Account_DAO> Accounts
        {
            get { return _accounts; }
            set { _accounts = value; }
        }

        [BelongsTo("SubsidyProviderKey", NotNull = true)]
        [ValidateNonEmpty("Subsidy Provider is a mandatory field")]
        public virtual SubsidyProvider_DAO SubsidyProvider
        {
            get
            {
                return this._subsidyProvider;
            }
            set
            {
                this._subsidyProvider = value;
            }
        }

        [Property("GEPFMember", ColumnType = "Boolean", NotNull = true)]
        public virtual bool GEPFMember
        {
            get
            {
                return this._gepfMember;
            }
            set
            {
                this._gepfMember = value;
            }
        }
    }
}