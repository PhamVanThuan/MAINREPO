using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ValuationCottage_DAO describes the Extent, Roof Type and rate/sq m of a Cottage for a SAHL Manual Valuation.
    /// </summary>
    //[GenericTest(Globals.TestType.Find)]
    [ActiveRecord("ValuationCottage", Schema = "dbo", Lazy = true)]
    public partial class ValuationCottage_DAO : DB_2AM<ValuationCottage_DAO>
    {
        private double? _extent;

        private double? _rate;

        private Valuation_DAO _valuation;

        private ValuationRoofType_DAO _valuationRoofType;

        private int _valuationKey;

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Foreign, Column = "ValuationKey")]
        public virtual int Key
        {
            get { return _valuationKey; }
            set { _valuationKey = value; }
        }

        /// <summary>
        /// The size, in sq m, of the cottage.
        /// </summary>
        [Property("Extent")]
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
        [Property("Rate")]
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
        /// The Cottage is related to a single Valuation.
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
        /// The foreign key reference to the ValuationRoofType table.
        /// </summary>
        [BelongsTo("ValuationRoofTypeKey")]
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