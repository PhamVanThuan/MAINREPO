using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("ConditionSet", Schema = "dbo")]
    public partial class ConditionSet_DAO : DB_2AM<ConditionSet_DAO>
    {
        private string _description;

        private int _key;

        private IList<ConditionSetCondition_DAO> _conditionSetConditions;

        private IList<ConditionConfiguration_DAO> _conditionConfigurations;

        private IList<ConditionSetUIStatement_DAO> _conditionSetUIStatements;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "ConditionSetKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ConditionSetCondition_DAO), ColumnKey = "ConditionSetKey", Lazy = true, Table = "ConditionSetCondition", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ConditionSetCondition_DAO> ConditionSetConditions
        {
            get
            {
                return this._conditionSetConditions;
            }
            set
            {
                this._conditionSetConditions = value;
            }
        }

        [HasAndBelongsToMany(typeof(ConditionConfiguration_DAO), ColumnRef = "ConditionConfigurationKey", ColumnKey = "ConditionSetKey", Lazy = true, Schema = "dbo", Table = "ConditionConfigurationConditionSet")]
        public virtual IList<ConditionConfiguration_DAO> ConditionConfigurations
        {
            get
            {
                return this._conditionConfigurations;
            }
            set
            {
                this._conditionConfigurations = value;
            }
        }


        [HasMany(typeof(ConditionSetUIStatement_DAO), ColumnKey = "ConditionSetKey", Lazy = true, Table = "ConditionSetUIStatement", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ConditionSetUIStatement_DAO> ConditionSetUIStatements
        {
            get
            {
                return this._conditionSetUIStatements;
            }
            set
            {
                this._conditionSetUIStatements = value;
            }
        }
    }
}