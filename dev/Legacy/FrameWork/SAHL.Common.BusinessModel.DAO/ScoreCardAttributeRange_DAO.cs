using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("ScoreCardAttributeRange", Schema = "dbo")]
    public partial class ScoreCardAttributeRange_DAO : DB_2AM<ScoreCardAttributeRange_DAO>
    {
        private double? _min;

        private double? _max;

        private double _points;

        private int _key;

        private ScoreCardAttribute_DAO _scoreCardAttribute;

        [Property("Min", ColumnType = "Double")]
        public virtual double? Min
        {
            get
            {
                return this._min;
            }
            set
            {
                this._min = value;
            }
        }

        [Property("Max", ColumnType = "Double")]
        public virtual double? Max
        {
            get
            {
                return this._max;
            }
            set
            {
                this._max = value;
            }
        }

        [Property("Points", ColumnType = "Double", NotNull = true)]
        public virtual double Points
        {
            get
            {
                return this._points;
            }
            set
            {
                this._points = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "ScoreCardAttributeRangeKey", ColumnType = "Int32")]
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

        [BelongsTo("ScoreCardAttributeKey", NotNull = true)]
        public virtual ScoreCardAttribute_DAO ScoreCardAttribute
        {
            get
            {
                return this._scoreCardAttribute;
            }
            set
            {
                this._scoreCardAttribute = value;
            }
        }
    }
}