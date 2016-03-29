
using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("StageDefinitionStageDefinitionGroup", Schema = "dbo")]
    public partial class StageDefinitionStageDefinitionGroup_WTF_DAO : DB_Test_WTF<StageDefinitionStageDefinitionGroup_WTF_DAO>
    {

        private int _key;

        private IList<StageDefinitionComposite_WTF_DAO> _stageDefinitionComposites;

        private IList<StageTransition_WTF_DAO> _stageTransitions;

        private IList<StageTransitionComposite_WTF_DAO> _stageTransitionComposites;

        private StageDefinition_WTF_DAO _stageDefinition;

        private StageDefinitionGroup_WTF_DAO _stageDefinitionGroup;

        [PrimaryKey(PrimaryKeyType.Native, "StageDefinitionStageDefinitionGroupKey", ColumnType = "Int32")]
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

        [HasMany(typeof(StageDefinitionComposite_WTF_DAO), ColumnKey = "StageDefinitionStageDefinitionGroupCompositeKey", Table = "StageDefinitionComposite")]
        public virtual IList<StageDefinitionComposite_WTF_DAO> StageDefinitionComposites
        {
            get
            {
                return this._stageDefinitionComposites;
            }
            set
            {
                this._stageDefinitionComposites = value;
            }
        }

        [HasMany(typeof(StageTransition_WTF_DAO), ColumnKey = "StageDefinitionStageDefinitionGroupKey", Table = "StageTransition")]
        public virtual IList<StageTransition_WTF_DAO> StageTransitions
        {
            get
            {
                return this._stageTransitions;
            }
            set
            {
                this._stageTransitions = value;
            }
        }

        [HasMany(typeof(StageTransitionComposite_WTF_DAO), ColumnKey = "StageDefinitionStageDefinitionGroupKey", Table = "StageTransitionComposite")]
        public virtual IList<StageTransitionComposite_WTF_DAO> StageTransitionComposites
        {
            get
            {
                return this._stageTransitionComposites;
            }
            set
            {
                this._stageTransitionComposites = value;
            }
        }

        [BelongsTo("StageDefinitionKey", NotNull = true)]
        public virtual StageDefinition_WTF_DAO StageDefinition
        {
            get
            {
                return this._stageDefinition;
            }
            set
            {
                this._stageDefinition = value;
            }
        }

        [BelongsTo("StageDefinitionGroupKey", NotNull = true)]
        public virtual StageDefinitionGroup_WTF_DAO StageDefinitionGroup
        {
            get
            {
                return this._stageDefinitionGroup;
            }
            set
            {
                this._stageDefinitionGroup = value;
            }
        }
    }
}

