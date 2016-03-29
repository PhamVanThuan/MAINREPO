using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]//cause the rs - generic test doesnt fit in the table
    [ActiveRecord("ScoreCardAttribute", Schema = "dbo")]
    public partial class ScoreCardAttribute_DAO : DB_2AM<ScoreCardAttribute_DAO>
    {
        private ScoreCard_DAO _scoreCardKey;

        private string _code;

        private string _description;

        private int _key;

        private IList<ScoreCardAttributeRange_DAO> _scoreCardAttributeRanges;

        [BelongsTo("ScoreCardKey", NotNull = true)]
        public virtual ScoreCard_DAO ScoreCardKey
        {
            get
            {
                return this._scoreCardKey;
            }
            set
            {
                this._scoreCardKey = value;
            }
        }

        [Property("Code", ColumnType = "String", NotNull = true)]
        public virtual string Code
        {
            get
            {
                return this._code;
            }
            set
            {
                this._code = value;
            }
        }

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

        [PrimaryKey(PrimaryKeyType.Assigned, "ScoreCardAttributeKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ScoreCardAttributeRange_DAO), ColumnKey = "ScoreCardAttributeKey", Table = "ScoreCardAttributeRange")]
        public virtual IList<ScoreCardAttributeRange_DAO> ScoreCardAttributeRanges
        {
            get
            {
                return this._scoreCardAttributeRanges;
            }
            set
            {
                this._scoreCardAttributeRanges = value;
            }
        }
    }
}