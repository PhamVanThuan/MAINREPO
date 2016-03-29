using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("ScoreCard", Schema = "dbo")]
    public partial class ScoreCard_DAO : DB_2AM<ScoreCard_DAO>
    {
        private string _description;

        private double _basePoints;

        private System.DateTime _revisionDate;

        private int _key;

        private IList<ScoreCardAttribute_DAO> _scoreCardAttributes;

        private IList<RiskMatrixDimension_DAO> _riskMatrixDimensions;

        private GeneralStatus_DAO _generalStatus;

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

        [Property("BasePoints", ColumnType = "Double")]
        public virtual double BasePoints
        {
            get
            {
                return this._basePoints;
            }
            set
            {
                this._basePoints = value;
            }
        }

        [Property("RevisionDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime RevisionDate
        {
            get
            {
                return this._revisionDate;
            }
            set
            {
                this._revisionDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "ScoreCardKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ScoreCardAttribute_DAO), ColumnKey = "ScoreCardKey", Table = "ScoreCardAttribute")]
        public virtual IList<ScoreCardAttribute_DAO> ScoreCardAttributes
        {
            get
            {
                return this._scoreCardAttributes;
            }
            set
            {
                this._scoreCardAttributes = value;
            }
        }

        [HasAndBelongsToMany(typeof(RiskMatrixDimension_DAO), ColumnKey = "ScoreCardKey", Table = "RiskMatrixDimensionScoreCard", ColumnRef = "RiskMatrixDimensionKey")]
        public virtual IList<RiskMatrixDimension_DAO> RiskMatrixDimensions
        {
            get
            {
                return this._riskMatrixDimensions;
            }
            set
            {
                this._riskMatrixDimensions = value;
            }
        }

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }
    }
}