using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("StageDefinitionGroup", Schema = "dbo", Lazy = true)]
    public partial class StageDefinitionGroup_DAO : DB_2AM<StageDefinitionGroup_DAO>
    {
        private string _description;

        private GeneralStatus_DAO _generalStatus;

        //private int _parentKey;

        private int _key;

        private IList<StageDefinitionStageDefinitionGroup_DAO> _stageDefinitionStageDefinitionGroups;

        private GenericKeyType_DAO _genericKeyType;

        private StageDefinitionGroup_DAO _parentStageDefinitionGroup;

        private IList<StageDefinitionGroup_DAO> _childStageDefinitionGroups;

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
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

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
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

        [PrimaryKey(PrimaryKeyType.Assigned, "StageDefinitionGroupKey", ColumnType = "Int32")]
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

        [HasMany(typeof(StageDefinitionStageDefinitionGroup_DAO), ColumnKey = "StageDefinitionGroupKey", Table = "StageDefinitionStageDefinitionGroup", Inverse = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Lazy = true)]
        public virtual IList<StageDefinitionStageDefinitionGroup_DAO> StageDefinitionStageDefinitionGroups
        {
            get
            {
                return this._stageDefinitionStageDefinitionGroups;
            }
            set
            {
                this._stageDefinitionStageDefinitionGroups = value;
            }
        }

        [BelongsTo("GenericKeyTypeKey", NotNull = true)]
        [ValidateNonEmpty("Generic Key Type is a mandatory field")]
        public virtual GenericKeyType_DAO GenericKeyType
        {
            get
            {
                return this._genericKeyType;
            }
            set
            {
                this._genericKeyType = value;
            }
        }

        [BelongsTo("ParentKey")]
        public virtual StageDefinitionGroup_DAO ParentStageDefinitionGroup
        {
            get
            {
                return this._parentStageDefinitionGroup;
            }
            set
            {
                this._parentStageDefinitionGroup = value;
            }
        }

        [HasMany(typeof(StageDefinitionGroup_DAO), ColumnKey = "ParentKey", Table = "StageDefinitionGroup", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<StageDefinitionGroup_DAO> ChildStageDefinitionGroups
        {
            get
            {
                return this._childStageDefinitionGroups;
            }
            set
            {
                this._childStageDefinitionGroups = value;
            }
        }
    }
}