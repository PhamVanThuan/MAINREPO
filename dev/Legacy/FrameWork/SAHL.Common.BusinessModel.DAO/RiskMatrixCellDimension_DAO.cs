using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("RiskMatrixCellDimension", Schema = "dbo")]
    public partial class RiskMatrixCellDimension_DAO : DB_2AM<RiskMatrixCellDimension_DAO>
    {
        private int _key;

        private RiskMatrixCell_DAO _riskMatrixCell;

        private RiskMatrixDimension_DAO _riskMatrixDimension;

        private RiskMatrixRange_DAO _riskMatrixRange;

        [PrimaryKey(PrimaryKeyType.Assigned, "RiskMatrixCellDimensionKey", ColumnType = "Int32")]
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

        [BelongsTo("RiskMatrixCellKey", NotNull = true)]
        public virtual RiskMatrixCell_DAO RiskMatrixCell
        {
            get
            {
                return this._riskMatrixCell;
            }
            set
            {
                this._riskMatrixCell = value;
            }
        }

        [BelongsTo("RiskMatrixDimensionKey", NotNull = true)]
        public virtual RiskMatrixDimension_DAO RiskMatrixDimension
        {
            get
            {
                return this._riskMatrixDimension;
            }
            set
            {
                this._riskMatrixDimension = value;
            }
        }

        [BelongsTo("RiskMatrixRangeKey", NotNull = true)]
        public virtual RiskMatrixRange_DAO RiskMatrixRange
        {
            get
            {
                return this._riskMatrixRange;
            }
            set
            {
                this._riskMatrixRange = value;
            }
        }
    }
}