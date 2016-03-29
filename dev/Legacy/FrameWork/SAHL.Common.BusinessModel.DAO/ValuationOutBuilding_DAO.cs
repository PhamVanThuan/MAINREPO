using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ValuationOutbuilding_DAO describes the extent, rate and roof type of the Outbuilding associated to a SAHL Manual Valuation.
    /// </summary>
    //[GenericTest(Globals.TestType.Find)]
    [ActiveRecord("ValuationOutbuilding", Schema = "dbo", Lazy = true)]
    public partial class ValuationOutbuilding_DAO : DB_2AM<ValuationOutbuilding_DAO>
    {
        private double? _extent;

        private double? _rate;

        private int _key;

        private Valuation_DAO _valuation;

        private ValuationRoofType_DAO _valuationRoofType;

        /// <summary>
        /// The size, in sq m, of the Outbuilding.
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
        /// The replacement value per sq m for the Outbuilding.
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
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "ValuationOutbuildingKey", ColumnType = "Int32")]
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

        /// <summary>
        /// Foreign key reference to the Valuation table. Each Outbuilding can only belong to a single valuation.
        /// </summary>
        [BelongsTo("ValuationKey", NotNull = true)]
        [ValidateNonEmpty("Valuation is a mandatory field")]
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
        /// Foreign key reference to the ValuationRoofType table. Each Outbuilding can only belong to one ValuationRoofType.
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