using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("RiskMatrixDimensionScoreCard", Schema = "dbo")]
    public partial class RiskMatrixDimensionScoreCard_DAO : DB_2AM<RiskMatrixDimensionScoreCard_DAO>
    {
        private int _key;

        private RiskMatrixDimension_DAO _riskMatrixDimension;

        private ScoreCard_DAO _scoreCard;

        [PrimaryKey(PrimaryKeyType.Assigned, "RiskMatrixDimensionScoreCardKey", ColumnType = "Int32")]
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

        [BelongsTo("ScoreCardKey", NotNull = true)]
        public virtual ScoreCard_DAO ScoreCard
        {
            get
            {
                return this._scoreCard;
            }
            set
            {
                this._scoreCard = value;
            }
        }
    }
}