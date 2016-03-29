using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ValuationMainBuilding_DAO describes the extent, rate and roof type of the Main Building associated to a SAHL Manual Valuation.
    /// </summary>
    //[GenericTest(Globals.TestType.Find)]
    [ActiveRecord("ValuationMainBuilding", Schema = "dbo", Lazy = true)]
    public partial class ValuationMainBuilding_DAO : DB_2AM<ValuationMainBuilding_DAO>
    {
        private double? _extent;

        private double? _rate;

        private Valuation_DAO _valuation;

        private ValuationRoofType_DAO _valuationRoofType;

        private int _valuationKey;

        /// <summary>
        /// Primary Key, which is referring to the ValuationKey for the Valuation to which the Main Building belongs.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Foreign, Column = "ValuationKey")]
        public virtual int Key
        {
            get { return _valuationKey; }
            set { _valuationKey = value; }
        }

        /// <summary>
        /// The size, in sq m, of the Main Building.
        /// </summary>
        [Property("Extent", NotNull = false)]
        public virtual double? Extent
        {
            get
            {
                return this._extent;
            }
            set
            {
                this._extent = value;
            }
        }

        /// <summary>
        /// The replacement value per sq m for the Cottage.
        /// </summary>
        [Property("Rate", NotNull = false)]
        public virtual double? Rate
        {
            get
            {
                return this._rate;
            }
            set
            {
                this._rate = value;
            }
        }

        /// <summary>
        /// Each Main Building can only belong to a single SAHL Manual Valuation.
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

        /// <summary>
        /// The foreign key reference to the ValuationRoofType table. A Main Building can only have a single Roof Type.
        /// </summary>
        [BelongsTo("ValuationRoofTypeKey", NotNull = false)]
        public virtual ValuationRoofType_DAO ValuationRoofType
        {
            get
            {
                return this._valuationRoofType;
            }
            set
            {
                this._valuationRoofType = value;
            }
        }
    }
}