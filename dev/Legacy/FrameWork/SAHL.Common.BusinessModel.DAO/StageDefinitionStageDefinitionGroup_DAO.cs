using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Ties StageDefinition_DAO objects with StageDefinitionGroup_DAO objects.
    /// </summary>
    /// <remarks>
    /// <strong>Note: </strong> There is a unique constraint on StageDefinitionKey and StageDefinitionGroupKey
    /// which prevents us being able to do automated LoadSaveLoad tests.  Loading and saving is tested via a
    /// manual test.
    /// </remarks>
    [GenericTest(TestType.Find)]
    [ActiveRecord("StageDefinitionStageDefinitionGroup", Schema = "dbo", Lazy = true)]
    public partial class StageDefinitionStageDefinitionGroup_DAO : DB_2AM<StageDefinitionStageDefinitionGroup_DAO>
    {
        private int _key;

        private StageDefinitionGroup_DAO _stageDefinitionGroup;

        private StageDefinition_DAO _stageDefinition;

        private IList<StageDefinitionStageDefinitionGroup_DAO> _compositeChildDefinitions;

        private IList<StageDefinitionStageDefinitionGroup_DAO> _compositeParentDefinitions;

        [PrimaryKey(PrimaryKeyType.Assigned, "StageDefinitionStageDefinitionGroupKey", ColumnType = "Int32")]
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

        [BelongsTo("StageDefinitionGroupKey", NotNull = true)]
        [ValidateNonEmpty("Stage Definition Group is a mandatory field")]
        public virtual StageDefinitionGroup_DAO StageDefinitionGroup
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

        [BelongsTo("StageDefinitionKey", NotNull = true)]
        [ValidateNonEmpty("Stage Definition is a mandatory field")]
        public virtual StageDefinition_DAO StageDefinition
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

        [HasAndBelongsToMany(typeof(StageDefinitionStageDefinitionGroup_DAO), Table = "StageDefinitionComposite", ColumnKey = "StageDefinitionStageDefinitionGroupKey", ColumnRef = "StageDefinitionStageDefinitionGroupCompositeKey", Lazy = true)]
        public virtual IList<StageDefinitionStageDefinitionGroup_DAO> CompositeChildStageDefinitionStageDefinitionGroups
        {
            get
            {
                return _compositeChildDefinitions;
            }
            set
            {
                _compositeChildDefinitions = value;
            }
        }

        [HasAndBelongsToMany(typeof(StageDefinitionStageDefinitionGroup_DAO), Table = "StageDefinitionComposite", ColumnKey = "StageDefinitionStageDefinitionGroupCompositeKey", ColumnRef = "StageDefinitionStageDefinitionGroupKey", Lazy = true)]
        public virtual IList<StageDefinitionStageDefinitionGroup_DAO> CompositeParentStageDefinitionStageDefinitionGroups
        {
            get
            {
                return _compositeParentDefinitions;
            }
            set
            {
                _compositeParentDefinitions = value;
            }
        }
    }
}