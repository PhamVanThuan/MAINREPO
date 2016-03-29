using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferInformationPersonalLoan", Schema = "dbo")]
    public partial class ApplicationInformationPersonalLoan_DAO : DB_2AM<ApplicationInformationPersonalLoan_DAO>
    {
        private double _loanAmount;

        private int _term;

        private double _monthlyInstalment;

        private double _lifePremium;

        private double _feesTotal;

        private int _applicationInformationKey;

        private CreditCriteriaUnsecuredLending_DAO _creditCriteriaUnsecuredLending;

        private Margin_DAO _margin;

        private MarketRate_DAO _marketRate;

        private ApplicationInformation_DAO _applicationInformation;

        /// <summary>
        /// Primary Key. This is also a foreign key reference to the OfferInformation table.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Foreign, Column = "OfferInformationKey")]
        public virtual int Key
        {
            get { return _applicationInformationKey; }
            set { _applicationInformationKey = value; }
        }

        [Property("LoanAmount", ColumnType = "Double", NotNull = true)]
        public virtual double LoanAmount
        {
            get
            {
                return this._loanAmount;
            }
            set
            {
                this._loanAmount = value;
            }
        }

        [Property("Term", ColumnType = "Int32", NotNull = true)]
        public virtual int Term
        {
            get
            {
                return this._term;
            }
            set
            {
                this._term = value;
            }
        }

        [Property("MonthlyInstalment", ColumnType = "Double", NotNull = true)]
        public virtual double MonthlyInstalment
        {
            get
            {
                return this._monthlyInstalment;
            }
            set
            {
                this._monthlyInstalment = value;
            }
        }

        [Property("LifePremium", ColumnType = "Double")]
        public virtual double LifePremium
        {
            get
            {
                return this._lifePremium;
            }
            set
            {
                this._lifePremium = value;
            }
        }

        [Property("FeesTotal", ColumnType = "Double", NotNull = true)]
        public virtual double FeesTotal
        {
            get
            {
                return this._feesTotal;
            }
            set
            {
                this._feesTotal = value;
            }
        }

        [BelongsTo("CreditCriteriaUnsecuredLendingKey", NotNull = true)]
        public virtual CreditCriteriaUnsecuredLending_DAO CreditCriteriaUnsecuredLending
        {
            get
            {
                return this._creditCriteriaUnsecuredLending;
            }
            set
            {
                this._creditCriteriaUnsecuredLending = value;
            }
        }

        [BelongsTo("MarginKey", NotNull = true)]
        public virtual Margin_DAO Margin
        {
            get
            {
                return this._margin;
            }
            set
            {
                this._margin = value;
            }
        }

        [BelongsTo("MarketRateKey", NotNull = true)]
        public virtual MarketRate_DAO MarketRate
        {
            get
            {
                return this._marketRate;
            }
            set
            {
                this._marketRate = value;
            }
        }

        [OneToOne]
        public virtual ApplicationInformation_DAO ApplicationInformation
        {
            get
            {
                return this._applicationInformation;
            }
            set
            {
                this._applicationInformation = value;
            }
        }

        // TODO : Zensar will need to complete
        public virtual void Clone(ApplicationInformationPersonalLoan_DAO personalLoan)
        {
        }
    }
}