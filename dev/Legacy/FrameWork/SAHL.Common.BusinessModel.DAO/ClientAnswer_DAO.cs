using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ClientAnswer", Schema = "Survey", Lazy = true)]
    public partial class ClientAnswer_DAO : DB_2AM<ClientAnswer_DAO>
    {
        private int _key;

        private ClientAnswerValue_DAO _clientAnswerValue;

        private Answer_DAO _answer;

        private QuestionnaireQuestion_DAO _questionnaireQuestion;

        private ClientQuestionnaire_DAO _clientSurvey;

        [PrimaryKey(PrimaryKeyType.Native, "ClientAnswerKey", ColumnType = "Int32")]
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

        [OneToOne(Cascade = CascadeEnum.All)]
        public virtual ClientAnswerValue_DAO ClientAnswerValue
        {
            get
            {
                return this._clientAnswerValue;
            }
            set
            {
                this._clientAnswerValue = value;
            }
        }

        [BelongsTo("AnswerKey", NotNull = true)]
        public virtual Answer_DAO Answer
        {
            get
            {
                return this._answer;
            }
            set
            {
                this._answer = value;
            }
        }

        [BelongsTo("QuestionnaireQuestionKey", NotNull = true)]
        public virtual QuestionnaireQuestion_DAO QuestionnaireQuestion
        {
            get
            {
                return this._questionnaireQuestion;
            }
            set
            {
                this._questionnaireQuestion = value;
            }
        }

        [BelongsTo("ClientQuestionnaireKey", NotNull = true)]
        public virtual ClientQuestionnaire_DAO ClientSurvey
        {
            get
            {
                return this._clientSurvey;
            }
            set
            {
                this._clientSurvey = value;
            }
        }
    }
}