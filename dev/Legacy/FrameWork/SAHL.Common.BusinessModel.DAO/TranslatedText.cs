using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("TranslatedText", Schema = "dbo", Lazy = true)]
    public partial class TranslatedText_DAO : DB_2AM<TranslatedText_DAO>
    {
        private string _translatedText;

        private int _key;

        private TranslatableItem_DAO _translatableItem;

        private Language_DAO _language;

        [Property("TranslatedText", ColumnType = "String")]
        public virtual string Text
        {
            get
            {
                return this._translatedText;
            }
            set
            {
                this._translatedText = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "TranslatedTextKey", ColumnType = "Int32")]
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

        [BelongsTo("TranslatableItemKey", NotNull = true)]
        [ValidateNonEmpty("Translatable Item is a mandatory field")]
        public virtual TranslatableItem_DAO TranslatableItem
        {
            get
            {
                return this._translatableItem;
            }
            set
            {
                this._translatableItem = value;
            }
        }

        [BelongsTo("LanguageKey", NotNull = true)]
        [ValidateNonEmpty("Language is a mandatory field")]
        public virtual Language_DAO Language
        {
            get
            {
                return this._language;
            }
            set
            {
                this._language = value;
            }
        }
    }
}