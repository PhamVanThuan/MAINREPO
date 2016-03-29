using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ValuationClassification_DAO is the Property Classification captured during a SAHL Manual Valuation.
    /// </summary>
    [ActiveRecord("ValuationClassification", Schema = "dbo", Lazy = true)]
    [Lookup]
    public partial class ValuationClassification_DAO : DB_2AM<ValuationClassification_DAO>
    {
        private string _description;

        private int _key;

        // commented, this is a lookup.
        //private IList<Valuation> _valuations;
        /// <summary>
        /// The Description of the ValuationClassification_DAO. e.g. Budget Standard.
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
        [PrimaryKey(PrimaryKeyType.Native, "ValuationClassificationKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(Valuation), ColumnKey = "ValuationClassificationKey", Table = "Valuation")]
        //public virtual IList<Valuation> Valuations
        //{
        //    get
        //    {
        //        return this._valuations;
        //    }
        //    set
        //    {
        //        this._valuations = value;
        //    }
        //}
    }
}