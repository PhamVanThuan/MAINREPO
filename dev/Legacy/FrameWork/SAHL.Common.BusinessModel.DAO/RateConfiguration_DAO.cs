using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Rateconfiguration is used to expose available MarketRate / Margin combinations. Each mortgageLoan links to a RateConfiguration Entry,
    /// but a list of MortgageLoans is not exposed here as this is a lookup.
    /// </summary>
    /// <seealso cref="MortgageLoan_DAO"/>
    /// <seealso cref="Margin_DAO"/>
    /// <seealso cref="MarketRate_DAO"/>

    [GenericTest(TestType.Find)]
    [ActiveRecord("RateConfiguration", Schema = "dbo", Lazy = true)]
    public partial class RateConfiguration_DAO : DB_2AM<RateConfiguration_DAO>
    {
        private int _key;

        // Commented; this is a lookup.
        //private IList<MortgageLoan_DAO> _mortgageLoans;

        private Margin_DAO _margin;

        private MarketRate_DAO _marketRate;

        /// <summary>
        /// This is the primary key, used to identify an instance of RateConfiguration.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "RateConfigurationKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(MortgageLoan_DAO), ColumnKey = "RateConfigurationKey", Table = "MortgageLoan", Lazy = true)]
        //public virtual IList<MortgageLoan_DAO> MortgageLoans
        //{
        //    get
        //    {
        //        return this._mortgageLoans;
        //    }
        //    set
        //    {
        //        this._mortgageLoans = value;
        //    }
        //}

        /// <summary>
        /// The margin.
        /// </summary>
        [BelongsTo("MarginKey", NotNull = true)]
        [ValidateNonEmpty("Margin is a mandatory field")]
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

        // todo: uncomment when marketrate is implemented.
        /// <summary>
        /// The MarketRate.
        /// </summary>
        [BelongsTo("MarketRateKey", NotNull = true)]
        [ValidateNonEmpty("Market Rate is a mandatory field")]
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
    }
}