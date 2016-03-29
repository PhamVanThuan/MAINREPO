using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("StageDefinition", Schema = "dbo", DiscriminatorColumn = "IsComposite", DiscriminatorType = "Boolean", DiscriminatorValue = "False", Lazy = false)]
    public partial class StageDefinition_DAO : DB_2AM<StageDefinition_DAO>
    {
        private string _description;

        private GeneralStatus_DAO _generalStatus;

        private string _name;

        private int _key;

        private bool _hasCompositeLogic;

        private IList<StageDefinitionStageDefinitionGroup_DAO> _stageDefinitionStageDefinitionGroups;

        [Property("HasCompositeLogic", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Has Composite Logic is a mandatory field")]
        public virtual bool HasCompositeLogic
        {
            get
            {
                return this._hasCompositeLogic;
            }
            set
            {
                this._hasCompositeLogic = value;
            }
        }

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

        [Property("Name", ColumnType = "String", NotNull = true, Unique = true)]
        [ValidateNonEmpty("Name is a mandatory field")]
        public virtual string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "StageDefinitionKey", ColumnType = "Int32")]
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
    }
}