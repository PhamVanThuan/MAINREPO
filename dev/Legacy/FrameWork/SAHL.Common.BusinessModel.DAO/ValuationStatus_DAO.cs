using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ValuationStatus_DAO describes the status of a Valuation. A valuation can currently be pending or complete.
    /// </summary>
    [GenericTest(TestType = TestType.Find)]
    [ActiveRecord("ValuationStatus", Schema = "dbo", Lazy = true)]
    public partial class ValuationStatus_DAO : DB_2AM<ValuationStatus_DAO>
    {
        private string _description;
        private int _key;

        /// <summary>
        /// Valuation status description.
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
        [PrimaryKey(PrimaryKeyType.Assigned, "ValuationStatusKey", ColumnType = "Int32")]
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
    }
}