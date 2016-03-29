using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ValuationRoofType_DAO describes the different roof types available for SAHL Manual Valuations.
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("ValuationRoofType", Schema = "dbo", Lazy = true)]
    public partial class ValuationRoofType_DAO : DB_2AM<ValuationRoofType_DAO>
    {
        private string _description;

        private int _key;

        // commeted, this is a lookup.
        //private IList<ValuationCottage> _valuationCottages;

        //private IList<ValuationMainBuilding> _valuationMainBuildings;

        //private IList<ValuationOutbuilding> _valuationOutbuildings;
        /// <summary>
        /// ValuationRoofType Description
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
        [PrimaryKey(PrimaryKeyType.Assigned, "ValuationRoofTypeKey", ColumnType = "Int32")]
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

        /*   Commented, this is a lookup.
        [HasMany(typeof(ValuationCottage), ColumnKey = "ValuationRoofTypeKey", Table = "ValuationCottage")]
        public virtual IList<ValuationCottage> ValuationCottages
        {
            get
            {
                return this._valuationCottages;
            }
            set
            {
                this._valuationCottages = value;
            }
        }

        [HasMany(typeof(ValuationMainBuilding), ColumnKey = "ValuationRoofTypeKey", Table = "ValuationMainBuilding")]
        public virtual IList<ValuationMainBuilding> ValuationMainBuildings
        {
            get
            {
                return this._valuationMainBuildings;
            }
            set
            {
                this._valuationMainBuildings = value;
            }
        }

        [HasMany(typeof(ValuationOutbuilding), ColumnKey = "ValuationRoofTypeKey", Table = "ValuationOutbuilding")]
        public virtual IList<ValuationOutbuilding> ValuationOutbuildings
        {
            get
            {
                return this._valuationOutbuildings;
            }
            set
            {
                this._valuationOutbuildings = value;
            }
        }

*/
    }
}