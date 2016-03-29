using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("TextStatementType", Schema = "dbo")]
    public partial class TextStatementType_DAO : DB_2AM<TextStatementType_DAO>
    {
        private string _description;

        private int _key;

        // Commented, this is a lookup.
        //private IList<TextStatement> _textStatements;

        private OriginationSourceProduct_DAO _originationSourceProduct;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "TextStatementTypeKey", ColumnType = "Int32")]
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
        //[HasMany(typeof(TextStatement), ColumnKey = "TextStatementTypeKey", Table = "TextStatement")]
        //public virtual IList<TextStatement> TextStatements
        //{
        //    get
        //    {
        //        return this._textStatements;
        //    }
        //    set
        //    {
        //        this._textStatements = value;
        //    }
        //}

        [BelongsTo("OriginationSourceProductKey", NotNull = true)]
        [ValidateNonEmpty("Origination Source Product is a mandatory field")]
        public virtual OriginationSourceProduct_DAO OriginationSourceProduct
        {
            get
            {
                return this._originationSourceProduct;
            }
            set
            {
                this._originationSourceProduct = value;
            }
        }
    }
}