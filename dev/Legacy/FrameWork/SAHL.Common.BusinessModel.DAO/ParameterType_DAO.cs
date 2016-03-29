using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ParameterType", Schema = "dbo", Lazy = false)]
    public partial class ParameterType_DAO : DB_2AM<ParameterType_DAO>
    {
        private string _description;

        private string _sQLDataType;

        private string _cSharpDataType;

        private int _key;

        // Commented as this is a lookup
        //private IList<StatementParameter_DAO> _statementParameters;

        //private IList<Token_DAO> _tokens;

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

        [Property("SQLDataType", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("SQL Data Type is a mandatory field")]
        public virtual string SQLDataType
        {
            get
            {
                return this._sQLDataType;
            }
            set
            {
                this._sQLDataType = value;
            }
        }

        [Property("CSharpDataType", ColumnType = "String")]
        public virtual string CSharpDataType
        {
            get
            {
                return this._cSharpDataType;
            }
            set
            {
                this._cSharpDataType = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ParameterTypeKey", ColumnType = "Int32")]
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

        // Commented as this is a lookup
        //[HasMany(typeof(StatementParameter_DAO), ColumnKey = "ParameterTypeKey", Lazy = true, Table = "StatementParameter")]
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

        //[HasMany(typeof(Token_DAO), ColumnKey = "ParameterTypeKey", Lazy = true, Table = "Token")]
        //public virtual IList<Token_DAO> Tokens
        //{
        //    get
        //    {
        //        return this._tokens;
        //    }
        //    set
        //    {
        //        this._tokens = value;
        //    }
        //}
    }
}