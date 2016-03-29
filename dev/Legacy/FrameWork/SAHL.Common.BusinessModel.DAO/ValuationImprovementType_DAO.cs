using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ValuationImprovementType_DAO describes the different types of improvements which can be captured against a SAHL Manual
    /// Valuation.
    /// </summary>
    [Lookup]
    [ActiveRecord("ValuationImprovementType", Schema = "dbo", Lazy = true)]
    public partial class ValuationImprovementType_DAO : DB_2AM<ValuationImprovementType_DAO>
    {
        private string _description;

        private int _key;

        /// <summary>
        /// The Improvement Description.
        /// </summary>
        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "ValuationImprovementTypeKey", ColumnType = "Int32")]
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

        // commented, this is a lookup.
        //[HasMany(typeof(ValuationImprovement), ColumnKey = "ValuationImprovementTypeKey", Table = "ValuationImprovement")]
        //public virtual IList<ValuationImprovement> ValuationImprovements
        //{
        //    get
        //    {
        //        return this._valuationImprovements;
        //    }
        //    set
        //    {
        //        this._valuationImprovements = value;
        //    }
        //}
    }
}