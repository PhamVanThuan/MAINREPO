using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("StatementDefinition", Schema = "dbo", Lazy = true)]
    public partial class StatementDefinition_DAO : DB_2AM<StatementDefinition_DAO>
    {
        private string _description;

        private string _applicationName;

        private string _statementName;

        private int _key;

        // commented, not sure we should have this link back.
        // private IList<StatementParameter_DAO> _statementParameters;

        private IList<StatementParameter_DAO> _statementParameters;

        private IList<Token_DAO> _tokens;

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

        [Property("ApplicationName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Application Name is a mandatory field")]
        public virtual string ApplicationName
        {
            get
            {
                return this._applicationName;
            }
            set
            {
                this._applicationName = value;
            }
        }

        [Property("StatementName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Statement Name is a mandatory field")]
        public virtual string StatementName
        {
            get
            {
                return this._statementName;
            }
            set
            {
                this._statementName = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "StatementDefinitionKey", ColumnType = "Int32")]
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

        // commented, not sure we should have this link back.
        //[HasMany(typeof(StatementParameter_DAO), ColumnKey = "PopulationStatementDefinitionKey", Lazy = true, Table = "StatementParameter")]
        //public virtual IList<StatementParameter_DAO> StatementParameters
        //{
        //    get
        //    {
        //        return this._statementParameters;
        //    }
        //    set
        //    {
        //        this._statementParameters = value;
        //    }
        //}

        [HasMany(typeof(StatementParameter_DAO), ColumnKey = "StatementDefinitionKey", Lazy = true, Table = "StatementParameter", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<StatementParameter_DAO> StatementParameters
        {
            get
            {
                return this._statementParameters;
            }
            set
            {
                this._statementParameters = value;
            }
        }

        [HasMany(typeof(Token_DAO), ColumnKey = "StatementDefinitionKey", Lazy = true, Table = "Token", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<Token_DAO> Tokens
        {
            get
            {
                return this._tokens;
            }
            set
            {
                this._tokens = value;
            }
        }
    }
}