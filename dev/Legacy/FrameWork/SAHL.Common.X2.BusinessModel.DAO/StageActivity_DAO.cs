using Castle.ActiveRecord;
using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [ActiveRecord("StageActivity", Schema = "X2")]
    public partial class StageActivity_DAO : DB_X2<StageActivity_DAO>
    {
        private int _stageDefinitionKey;

        private int _iD;

        private Activity_DAO _activity;

        private int _StageDefinitionStageDefinitionGroupKey;

        [Property("StageDefinitionKey", ColumnType = "Int32", NotNull = true)]
        public virtual int StageDefinitionKey
        {
            get
            {
                return this._stageDefinitionKey;
            }
            set
            {
                this._stageDefinitionKey = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ID", ColumnType = "Int32")]
        public virtual int ID
        {
            get
            {
                return this._iD;
            }
            set
            {
                this._iD = value;
            }
        }

        [BelongsTo("ActivityID", NotNull = true)]
        public virtual Activity_DAO Activity
        {
            get
            {
                return this._activity;
            }
            set
            {
                this._activity = value;
            }
        }

        [Property("StageDefinitionStageDefinitionGroupKey", ColumnType = "Int32", NotNull = true)]
        public virtual int StageDefinitionStageDefinitionGroupKey
        {
            get
            {
                return _StageDefinitionStageDefinitionGroupKey;
            }
            set
            {
                _StageDefinitionStageDefinitionGroupKey = value;
            }
        }
    }
}