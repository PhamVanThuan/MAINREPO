using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("QuestionnaireQuestionnaireEmail", Schema = "Survey", Lazy = true)]
    public partial class QuestionnaireQuestionnaireEmail_DAO : DB_2AM<QuestionnaireQuestionnaireEmail_DAO>
    {
        private int _key;

        private int _internalEmail;

        private Questionnaire_DAO _questionnaire;

        private QuestionnaireEmail_DAO _questionnaireEmail;

        [PrimaryKey(PrimaryKeyType.Native, "QuestionnaireQuestionnaireEmailKey", ColumnType = "Int32")]
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

        [Property("InternalEmail", ColumnType = "Int32")]
        public virtual int InternalEmail
        {
            get
            {
                return this._internalEmail;
            }
            set
            {
                this._internalEmail = value;
            }
        }

        [BelongsTo("QuestionnaireKey")]
        public virtual Questionnaire_DAO Questionnaire
        {
            get
            {
                return this._questionnaire;
            }
            set
            {
                this._questionnaire = value;
            }
        }

        [BelongsTo("QuestionnaireEmailKey")]
        public virtual QuestionnaireEmail_DAO QuestionnaireEmail
        {
            get
            {
                return this._questionnaireEmail;
            }
            set
            {
                this._questionnaireEmail = value;
            }
        }
    }
}