using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("uiStatementType", Schema = "dbo")]
    public partial class UIStatementType_DAO : DB_2AM<UIStatementType_DAO>
    {
        private string _description;

        private int _key;

        // commented, this is a lookup.
        //private IList<uiStatement> _uiStatements;

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

        [PrimaryKey(PrimaryKeyType.Native, "StatementTypeKey", ColumnType = "Int32")]
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

        // commented, this is a lookup.
        //[HasMany(typeof(uiStatement), ColumnKey = "Type", Table = "uiStatement")]
        //public virtual IList<uiStatement> uiStatements
        //{
        //    get
        //    {
        //        return this._uiStatements;
        //    }
        //    set
        //    {
        //        this._uiStatements = value;
        //    }
        //}
    }
}