using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("TokenType", Schema = "dbo")]
    public partial class TokenType_DAO : DB_2AM<TokenType_DAO>
    {
        private string _description;

        private string _userID;

        private bool _runStatement;

        private int _key;

        // Commmented as this is a lookup.
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

        [Property("UserID", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("User ID is a mandatory field")]
        public virtual string UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
            }
        }

        [Property("RunStatement", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Run Statement is a mandatory field")]
        public virtual bool RunStatement
        {
            get
            {
                return this._runStatement;
            }
            set
            {
                this._runStatement = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "TokenTypeKey", ColumnType = "Int32")]
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

        // Commmented as this is a lookup.
        //[HasMany(typeof(Token_DAO), ColumnKey = "TokenTypeKey", Lazy = true, Table = "Token")]
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