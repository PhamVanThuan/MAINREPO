using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("RiskMatrixDimension", Schema = "dbo")]
    public partial class RiskMatrixDimension_DAO : DB_2AM<RiskMatrixDimension_DAO>
    {
        private string _description;

        private int _key;

        private IList<ScoreCard_DAO> _ScoreCards;

        private IList<RiskMatrixRange_DAO> _riskMatrixRanges;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "RiskMatrixDimensionKey", ColumnType = "Int32")]
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

        [HasAndBelongsToMany(typeof(ScoreCard_DAO), Table = "RiskMatrixDimensionScoreCard", ColumnKey = "RiskMatrixDimensionKey", ColumnRef = "ScoreCardKey")]
        public virtual IList<ScoreCard_DAO> ScoreCards
        {
            get
            {
                return this._ScoreCards;
            }
            set
            {
                this._ScoreCards = value;
            }
        }

        [HasAndBelongsToMany(typeof(RiskMatrixRange_DAO), ColumnKey = "RiskMatrixDimensionKey", Table = "RiskMatrixCellDimension", ColumnRef = "RiskMatrixRangeKey")]
        public virtual IList<RiskMatrixRange_DAO> RiskMatrixRanges
        {
            get
            {
                return this._riskMatrixRanges;
            }
            set
            {
                this._riskMatrixRanges = value;
            }
        }
    }
}