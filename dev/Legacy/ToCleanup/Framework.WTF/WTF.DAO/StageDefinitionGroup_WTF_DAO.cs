
using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("StageDefinitionGroup", Schema = "dbo")]
    public partial class StageDefinitionGroup_WTF_DAO : DB_Test_WTF<StageDefinitionGroup_WTF_DAO>
    {

        private string _description;

        private int _genericKeyTypeKey;

        private int _key;

        private IList<StageDefinitionGroup_WTF_DAO> _stageDefinitionGroups;

        private IList<StageDefinitionStageDefinitionGroup_WTF_DAO> _stageDefinitionStageDefinitionGroups;

        private GeneralStatus_WTF_DAO _generalStatus;

        private StageDefinitionGroup_WTF_DAO _parentStageDefinitionGroup;

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

        [Property("GenericKeyTypeKey", ColumnType = "Int32", NotNull = true)]
        public virtual int GenericKeyTypeKey
        {
            get
            {
                return this._genericKeyTypeKey;
            }
            set
            {
                this._genericKeyTypeKey = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "StageDefinitionGroupKey", ColumnType = "Int32")]
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

        [HasMany(typeof(StageDefinitionGroup_WTF_DAO), ColumnKey = "ParentKey", Table = "StageDefinitionGroup")]
        public virtual IList<StageDefinitionGroup_WTF_DAO> StageDefinitionGroups
        {
            get
            {
                return this._stageDefinitionGroups;
            }
            set
            {
                this._stageDefinitionGroups = value;
            }
        }

        [HasMany(typeof(StageDefinitionStageDefinitionGroup_WTF_DAO), ColumnKey = "StageDefinitionGroupKey", Table = "StageDefinitionStageDefinitionGroup")]
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

        [BelongsTo("ParentKey")]
        public virtual StageDefinitionGroup_WTF_DAO ParentStageDefinitionGroup
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
    }
}

