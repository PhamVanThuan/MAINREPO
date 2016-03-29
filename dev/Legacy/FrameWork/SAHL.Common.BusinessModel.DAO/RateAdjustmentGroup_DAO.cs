using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("RateAdjustmentGroup", Schema = "dbo")]
    public partial class RateAdjustmentGroup_DAO : DB_2AM<RateAdjustmentGroup_DAO>
    {
        private string _description;

        private int _key;

        private IList<RateAdjustmentElement_DAO> _rateAdjustmentElements;

        [Property("Description", ColumnType = "String", NotNull = true)]
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

        [PrimaryKey(PrimaryKeyType.Assigned, "RateAdjustmentGroupKey", ColumnType = "Int32")]
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

        [HasMany(typeof(RateAdjustmentElement_DAO), ColumnKey = "RateAdjustmentGroupKey", Table = "RateAdjustmentElement")]
        public virtual IList<RateAdjustmentElement_DAO> RateAdjustmentElements
        {
            get
            {
                return this._rateAdjustmentElements;
            }
            set
            {
                this._rateAdjustmentElements = value;
            }
        }
    }
}