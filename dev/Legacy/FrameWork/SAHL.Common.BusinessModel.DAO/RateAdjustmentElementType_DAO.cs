using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("RateAdjustmentElementType", Schema = "dbo")]
    public partial class RateAdjustmentElementType_DAO : DB_2AM<RateAdjustmentElementType_DAO>
    {
        private string _description;

        private int _key;

        private string _statementName;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "RateAdjustmentElementTypeKey", ColumnType = "Int32")]
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

        [Property("StatementName", ColumnType = "String", NotNull = true)]
        public virtual string StatementName
        {
            get
            {
                return this._statementName;
            }
            set
            {
                this._statementName = value;
            }
        }

        [HasMany(typeof(RateAdjustmentElement_DAO), ColumnKey = "RateAdjustmentElementTypeKey", Table = "RateAdjustmentElement")]
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