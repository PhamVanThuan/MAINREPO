using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ConditionConfiguration", Schema = "dbo")]
    public partial class ConditionConfiguration_DAO : DB_2AM<ConditionConfiguration_DAO>
    {
        private int _genericColumnDefinitionValue;

        private int _key;

        private GenericKeyType_DAO _genericKeyType;

        private GenericColumnDefinition_DAO _genericColumnDefinition;

        private IList<ConditionSet_DAO> _conditionSets;

        private string _uiStatementName;

        [Property("GenericColumnDefinitionValue", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Generic Column Definition Value is a mandatory field")]
        public virtual int GenericColumnDefinitionValue
        {
            get
            {
                return this._genericColumnDefinitionValue;
            }
            set
            {
                this._genericColumnDefinitionValue = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ConditionConfigurationKey", ColumnType = "Int32")]
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

        [BelongsTo("GenericColumnDefinitionKey", NotNull = true)]
        [ValidateNonEmpty("Generic Column Definition is a mandatory field")]
        public virtual GenericColumnDefinition_DAO GenericColumnDefinition
        {
            get
            {
                return this._genericColumnDefinition;
            }
            set
            {
                this._genericColumnDefinition = value;
            }
        }

        [HasAndBelongsToMany(typeof(ConditionSet_DAO), ColumnRef = "ConditionSetKey", ColumnKey = "ConditionConfigurationKey", Lazy = true, Schema = "dbo", Table = "ConditionConfigurationConditionSet")]
        public virtual IList<ConditionSet_DAO> ConditionSets
        {
            get
            {
                return this._conditionSets;
            }
            set
            {
                this._conditionSets = value;
            }
        }
    }
}