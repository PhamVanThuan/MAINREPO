using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("TextStatement", Schema = "dbo")]
    public partial class TextStatement_DAO : DB_2AM<TextStatement_DAO>
    {
        private string _statementTitle;

        private string _statement;

        private int _key;

        private TextStatementType_DAO _textStatementType;

        [Property("StatementTitle", ColumnType = "String", NotNull = false)]
        public virtual string StatementTitle
        {
            get
            {
                return this._statementTitle;
            }
            set
            {
                this._statementTitle = value;
            }
        }

        [Property("Statement", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Statement is a mandatory field")]
        public virtual string Statement
        {
            get
            {
                return this._statement;
            }
            set
            {
                this._statement = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "TextStatementKey", ColumnType = "Int32")]
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

        [BelongsTo("TextStatementTypeKey", NotNull = true)]
        [ValidateNonEmpty("Text Statement Type is a mandatory field")]
        public virtual TextStatementType_DAO TextStatementType
        {
            get
            {
                return this._textStatementType;
            }
            set
            {
                this._textStatementType = value;
            }
        }
    }
}