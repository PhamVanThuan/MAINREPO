using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("LifePremiumForecast", Schema = "dbo")]
    public partial class LifePremiumForecast_DAO : DB_2AM<LifePremiumForecast_DAO>
    {
        private short _loanYear;

        private short _age;

        private double _sumAssured;

        private double _monthlyPremium;

        private double _yearlyPremium;

        private double _monthlyComm;

        private System.DateTime _entryDate;

        private int _key;

        private AccountLifePolicy_DAO _lifePolicy;

        [Property("LoanYear", ColumnType = "Int16", NotNull = true)]
        [ValidateNonEmpty("Loan Year is a mandatory field")]
        public virtual short LoanYear
        {
            get
            {
                return this._loanYear;
            }
            set
            {
                this._loanYear = value;
            }
        }

        [Property("Age", ColumnType = "Int16", NotNull = true)]
        [ValidateNonEmpty("Age is a mandatory field")]
        public virtual short Age
        {
            get
            {
                return this._age;
            }
            set
            {
                this._age = value;
            }
        }

        [Property("SumAssured", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Sum Assured is a mandatory field")]
        public virtual double SumAssured
        {
            get
            {
                return this._sumAssured;
            }
            set
            {
                this._sumAssured = value;
            }
        }

        [Property("MonthlyPremium", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Monthly Premium is a mandatory field")]
        public virtual double MonthlyPremium
        {
            get
            {
                return this._monthlyPremium;
            }
            set
            {
                this._monthlyPremium = value;
            }
        }

        [Property("YearlyPremium", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Yearly Premium is a mandatory field")]
        public virtual double YearlyPremium
        {
            get
            {
                return this._yearlyPremium;
            }
            set
            {
                this._yearlyPremium = value;
            }
        }

        [Property("MonthlyComm", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Monthly Commission is a mandatory field")]
        public virtual double MonthlyComm
        {
            get
            {
                return this._monthlyComm;
            }
            set
            {
                this._monthlyComm = value;
            }
        }

        [Property("EntryDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Entry Date is a mandatory field")]
        public virtual System.DateTime EntryDate
        {
            get
            {
                return this._entryDate;
            }
            set
            {
                this._entryDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "LifePremiumForecastKey", ColumnType = "Int32")]
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

        [BelongsTo("AccountKey", NotNull = true)]
        [ValidateNonEmpty("Life Policy is a mandatory field")]
        public virtual AccountLifePolicy_DAO LifePolicy
        {
            get
            {
                return this._lifePolicy;
            }
            set
            {
                this._lifePolicy = value;
            }
        }
    }
}