using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("TranslatableItem", Schema = "dbo", Lazy = true)]
    public partial class TranslatableItem_DAO : DB_2AM<TranslatableItem_DAO>
    {
        private string _description;

        private int _key;

        private IList<TranslatedText_DAO> _translatedTexts;

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

        [PrimaryKey(PrimaryKeyType.Native, "TranslatableItemKey", ColumnType = "Int32")]
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

        [HasMany(typeof(TranslatedText_DAO), ColumnKey = "TranslatableItemKey", Table = "TranslatedText", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<TranslatedText_DAO> TranslatedTexts
        {
            get
            {
                return this._translatedTexts;
            }
            set
            {
                this._translatedTexts = value;
            }
        }
    }
}