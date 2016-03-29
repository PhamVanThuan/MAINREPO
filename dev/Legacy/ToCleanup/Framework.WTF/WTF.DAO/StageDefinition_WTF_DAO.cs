
using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("StageDefinition", Schema = "dbo")]
    public partial class StageDefinition_WTF_DAO : DB_Test_WTF<StageDefinition_WTF_DAO>
    {

        private string _description;

        private bool _isComposite;

        private string _name;

        private string _compositeTypeName;

        private bool _hasCompositeLogic;

        private int _key;

        private IList<StageDefinitionStageDefinitionGroup_WTF_DAO> _stageDefinitionStageDefinitionGroups;

        private GeneralStatus_WTF_DAO _generalStatus;

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

        [Property("IsComposite", ColumnType = "Boolean", NotNull = true)]
        public virtual bool IsComposite
        {
            get
            {
                return this._isComposite;
            }
            set
            {
                this._isComposite = value;
            }
        }

        [Property("Name", ColumnType = "String", NotNull = true)]
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

        [Property("CompositeTypeName", ColumnType = "String")]
        public virtual string CompositeTypeName
        {
            get
            {
                return this._compositeTypeName;
            }
            set
            {
                this._compositeTypeName = value;
            }
        }

        [Property("HasCompositeLogic", ColumnType = "Boolean")]
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

        [PrimaryKey(PrimaryKeyType.Native, "StageDefinitionKey", ColumnType = "Int32")]
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

        [HasMany(typeof(StageDefinitionStageDefinitionGroup_WTF_DAO), ColumnKey = "StageDefinitionKey", Table = "StageDefinitionStageDefinitionGroup")]
        public virtual IList<StageDefinitionStageDefinitionGroup_WTF_DAO> StageDefinitionStageDefinitionGroups
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

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        public virtual GeneralStatus_WTF_DAO GeneralStatus
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

