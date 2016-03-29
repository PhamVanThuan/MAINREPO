using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("QuestionnaireEmail", Schema = "Survey", Lazy = true)]
    public partial class QuestionnaireEmail_DAO : DB_2AM<QuestionnaireEmail_DAO>
    {
        private int _key;

        private string _emailSubject;

        private string _emailBody;

        private ContentType_DAO _contentType;

        private IList<QuestionnaireQuestionnaireEmail_DAO> _questionnaireQuestionnaireEmails;

        [PrimaryKey(PrimaryKeyType.Native, "QuestionnaireEmailKey", ColumnType = "Int32")]
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

        [Property("EmailSubject", ColumnType = "StringClob", NotNull = true)]
        public virtual string EmailSubject
        {
            get
            {
                return this._emailSubject;
            }
            set
            {
                this._emailSubject = value;
            }
        }

        [Property("EmailBody", ColumnType = "StringClob", NotNull = true)]
        public virtual string EmailBody
        {
            get
            {
                return this._emailBody;
            }
            set
            {
                this._emailBody = value;
            }
        }

        [BelongsTo(Column = "ContentTypeKey")]
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

        [HasMany(typeof(QuestionnaireQuestionnaireEmail_DAO), ColumnKey = "QuestionnaireEmailKey", Table = "QuestionnaireQuestionnaireEmail")]
        public virtual IList<QuestionnaireQuestionnaireEmail_DAO> QuestionnaireQuestionnaireEmails
        {
            get
            {
                return this._questionnaireQuestionnaireEmails;
            }
            set
            {
                this._questionnaireQuestionnaireEmails = value;
            }
        }
    }
}