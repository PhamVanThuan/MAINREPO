using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("CorrespondenceTemplate", Schema = "dbo", Lazy = true)]
    public partial class CorrespondenceTemplate_DAO : DB_2AM<CorrespondenceTemplate_DAO>
    {
        private int _key;

        private string _name;

        private string _subject;

        private string _template;

        private string _defaultEmail;

        private ContentType_DAO _contentType;

        [PrimaryKey(PrimaryKeyType.Assigned, "CorrespondenceTemplateKey", ColumnType = "Int32")]
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

        [Property("Name", ColumnType = "String", NotNull = true)]
        public virtual string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [Property("Subject", ColumnType = "String", NotNull = true)]
        public virtual string Subject
        {
            get
            {
                return this._subject;
            }
            set
            {
                this._subject = value;
            }
        }

        [Property("Template", ColumnType = "String", NotNull = true)]
        public virtual string Template
        {
            get
            {
                return this._template;
            }
            set
            {
                this._template = value;
            }
        }

        [Property("DefaultEmail", ColumnType = "String", NotNull = false)]
        public virtual string DefaultEmail
        {
            get
            {
                return this._defaultEmail;
            }
            set
            {
                this._defaultEmail = value;
            }
        }

        [BelongsTo("ContentTypeKey", NotNull = true)]
        [ValidateNonEmpty("Content Type is a mandatory field")]
        public virtual ContentType_DAO ContentType
        {
            get
            {
                return this._contentType;
            }
            set
            {
                this._contentType = value;
            }
        }
    }
}