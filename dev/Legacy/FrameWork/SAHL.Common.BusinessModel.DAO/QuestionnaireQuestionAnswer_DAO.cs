using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("QuestionnaireQuestionAnswer", Schema = "Survey", Lazy = true)]
    public partial class QuestionnaireQuestionAnswer_DAO : DB_2AM<QuestionnaireQuestionAnswer_DAO>
    {
        private int _key;

        private int _sequence;

        private Answer_DAO _answer;

        private QuestionnaireQuestion_DAO _questionnaireQuestion;

        [PrimaryKey(PrimaryKeyType.Native, "QuestionnaireQuestionAnswerKey", ColumnType = "Int32")]
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

        [Property("Sequence", ColumnType = "Int32", NotNull = true)]
        public virtual int Sequence
        {
            get
            {
                return this._sequence;
            }
            set
            {
                this._sequence = value;
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
    }
}