using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    //[DoNotTestWithGenericTest]
    [ActiveRecord("LifePremiumHistory", Schema = "dbo")]
    public partial class LifePremiumHistory_DAO : DB_2AM<LifePremiumHistory_DAO>
    {
        private int _Key;

        private System.DateTime _changeDate;

        private double _deathPremium;

        private double _iPBPremium;

        private double _sumAssured;

        private double _yearlyPremium;

        private double _policyFactor;

        private double _discountFactor;

        private string _userName;

        //private string _note;

        private double _monthlyPremium;

        private Account_DAO _account;

        [PrimaryKey(PrimaryKeyType.Native, "LifePremiumHistoryKey", ColumnType = "Int32")]
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

        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Change Date is a mandatory field")]
        public virtual System.DateTime ChangeDate
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

        [Property("DeathPremium", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Death Premium is a mandatory field")]
        public virtual double DeathPremium
        {
            get
            {
                return this._deathPremium;
            }
            set
            {
                this._deathPremium = value;
            }
        }

        [Property("IPBPremium", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("IPB Premium is a mandatory field")]
        public virtual double IPBPremium
        {
            get
            {
                return this._iPBPremium;
            }
            set
            {
                this._iPBPremium = value;
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

        [Property("PolicyFactor", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Policy Factor is a mandatory field")]
        public virtual double PolicyFactor
        {
            get
            {
                return this._policyFactor;
            }
            set
            {
                this._policyFactor = value;
            }
        }

        [Property("DiscountFactor", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Discount Factor is a mandatory field")]
        public virtual double DiscountFactor
        {
            get
            {
                return this._discountFactor;
            }
            set
            {
                this._discountFactor = value;
            }
        }

        [Property("MonthlyPremium", ColumnType = "Double", NotNull = false)]
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

        [Property("UserName", ColumnType = "String")]
        public virtual string UserName
        {
            get
            {
                return this._userName;
            }
            set
            {
                this._userName = value;
            }
        }

        //[Property("Note", ColumnType = "String")]
        //public virtual string Note
        //{
        //    get
        //    {
        //        return this._note;
        //    }
        //    set
        //    {
        //        this._note = value;
        //    }
        //}

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
    }
}