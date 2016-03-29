using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ValuationCombinedThatch_DAO stores the Combined Total Thatch Value of the SAHL Manual Valuation where the roof type
    /// is Thatch.
    /// </summary>
    //[GenericTest(Globals.TestType.Find)]
    [ActiveRecord("ValuationCombinedThatch", Schema = "dbo", Lazy = true)]
    public partial class ValuationCombinedThatch_DAO : DB_2AM<ValuationCombinedThatch_DAO>
    {
        private double _value;

        private int _valuationKey;

        private Valuation_DAO _valuation;

        /// <summary>
        /// The Combined Total Thatch Value
        /// </summary>
        [Property("Value", ColumnType = "Double")]
        public virtual double Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Foreign, "ValuationKey")]
        public virtual int Key
        {
            get
            {
                return this._valuationKey;
            }
            set
            {
                this._valuationKey = value;
            }
        }

        /// <summary>
        /// The Combined Total Thatch Value is related to a single Valuation.
        /// </summary>
        [OneToOne]
        public virtual Valuation_DAO Valuation
        {
            get
            {
                return this._valuation;
            }
            set
            {
                this._valuation = value;
            }
        }
    }
}