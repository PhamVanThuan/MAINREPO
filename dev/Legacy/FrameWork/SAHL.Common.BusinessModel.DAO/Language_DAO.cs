using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("Language", Schema = "dbo", Lazy = true)]
    public partial class Language_DAO : DB_2AM<Language_DAO>
    {
        private string _description;

        private bool _translatable;

        private int _key;

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

        [Property("Translatable", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Translatable is a mandatory field")]
        public virtual bool Translatable
        {
            get
            {
                return this._translatable;
            }
            set
            {
                this._translatable = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "LanguageKey", ColumnType = "Int32")]
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
    }
}