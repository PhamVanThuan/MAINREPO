using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ValuationImprovement_DAO describes improvements, the extent (where applicable) and the replacement rate/value
    /// of the improvements captured for a SAHL Manual Valuation.
    /// </summary>
    /// <seealso cref="ValuationImprovementType_DAO"/>
    //[GenericTest(Globals.TestType.Find)]
    [ActiveRecord("ValuationImprovement", Schema = "dbo", Lazy = true)]
    public partial class ValuationImprovement_DAO : DB_2AM<ValuationImprovement_DAO>
    {
        private System.DateTime? _improvementDate;

        private double _improvementValue;

        private int _key;

        private Valuation_DAO _valuation;

        private ValuationImprovementType_DAO _valuationImprovementType;

        /// <summary>
        /// The date on which the Improvement was added.
        /// </summary>
        [Property("ImprovementDate")]
        public virtual System.DateTime? ImprovementDate
        {
            get
            {
                return this._improvementDate;
            }
            set
            {
                this._improvementDate = value;
            }
        }

        [Property("ImprovementValue", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Improvement Value is a mandatory field")]
        public virtual double ImprovementValue
        {
            get
            {
                return this._improvementValue;
            }
            set
            {
                this._improvementValue = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "ValuationImprovementKey", ColumnType = "Int32")]
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
        /// An Improvement may only be related to a single Valuation.
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
        /// The foreign key reference to the ValuationImprovementType table. Each Improvement requires a Type.
        /// </summary>
        [BelongsTo("ValuationImprovementTypeKey", NotNull = true)]
        [ValidateNonEmpty("Valuation Improvement Type is a mandatory field")]
        public virtual ValuationImprovementType_DAO ValuationImprovementType
        {
            get
            {
                return this._valuationImprovementType;
            }
            set
            {
                this._valuationImprovementType = value;
            }
        }
    }
}