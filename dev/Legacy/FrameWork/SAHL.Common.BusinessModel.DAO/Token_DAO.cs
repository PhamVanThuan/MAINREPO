using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Token", Schema = "dbo")]
    public partial class Token_DAO : DB_2AM<Token_DAO>
    {
        private string _description;

        private bool _mustTranslate;

        private int _key;

        private IList<ConditionToken_DAO> _conditionTokens;

        private ParameterType_DAO _parameterType;

        private StatementDefinition_DAO _statementDefinition;

        private TokenType_DAO _tokenType;

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

        [Property("MustTranslate", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Must Translate is a mandatory field")]
        public virtual bool MustTranslate
        {
            get
            {
                return this._mustTranslate;
            }
            set
            {
                this._mustTranslate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "TokenKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ConditionToken_DAO), ColumnKey = "TokenKey", Lazy = true, Table = "ConditionToken", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ConditionToken_DAO> ConditionTokens
        {
            get
            {
                return this._conditionTokens;
            }
            set
            {
                this._conditionTokens = value;
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

        [BelongsTo("StatementDefinitionKey", NotNull = false)]
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

        [BelongsTo("TokenTypeKey", NotNull = true)]
        [ValidateNonEmpty("Token Type is a mandatory field")]
        public virtual TokenType_DAO TokenType
        {
            get
            {
                return this._tokenType;
            }
            set
            {
                this._tokenType = value;
            }
        }
    }
}