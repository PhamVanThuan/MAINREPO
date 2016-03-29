using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("StatementParameter", Schema = "dbo", Lazy = true)]
    public partial class StatementParameter_DAO : DB_2AM<StatementParameter_DAO>
    {
        private string _parameterName;

        private int? _parameterLength;

        private string _displayName;

        private bool _required;

        private int _key;

        private ParameterType_DAO _parameterType;

        private StatementDefinition_DAO _statementDefinition;

        private StatementDefinition_DAO _populationStatementDefinition;

        [Property("ParameterName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Parameter Name is a mandatory field")]
        public virtual string ParameterName
        {
            get
            {
                return this._parameterName;
            }
            set
            {
                this._parameterName = value;
            }
        }

        [Property("ParameterLength")]
        public virtual int? ParameterLength
        {
            get
            {
                return this._parameterLength;
            }
            set
            {
                this._parameterLength = value;
            }
        }

        [Property("DisplayName", ColumnType = "String")]
        public virtual string DisplayName
        {
            get
            {
                return this._displayName;
            }
            set
            {
                this._displayName = value;
            }
        }

        [Property("Required", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Required is a mandatory field")]
        public virtual bool Required
        {
            get
            {
                return this._required;
            }
            set
            {
                this._required = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "StatementParameterKey", ColumnType = "Int32")]
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

        [BelongsTo("ParameterTypeKey", NotNull = true)]
        [ValidateNonEmpty("Parameter Type is a mandatory field")]
        public virtual ParameterType_DAO ParameterType
        {
            get
            {
                return this._parameterType;
            }
            set
            {
                this._parameterType = value;
            }
        }

        [BelongsTo("PopulationStatementDefinitionKey", NotNull = false)]
        public virtual StatementDefinition_DAO PopulationStatementDefinition
        {
            get
            {
                return this._populationStatementDefinition;
            }
            set
            {
                this._populationStatementDefinition = value;
            }
        }

        [BelongsTo("StatementDefinitionKey", NotNull = true)]
        [ValidateNonEmpty("Statement Definition is a mandatory field")]
        public virtual StatementDefinition_DAO StatementDefinition
        {
            get
            {
                return this._statementDefinition;
            }
            set
            {
                this._statementDefinition = value;
            }
        }
    }
}