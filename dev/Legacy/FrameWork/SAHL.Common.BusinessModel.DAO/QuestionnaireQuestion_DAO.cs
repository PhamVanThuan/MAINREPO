using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("QuestionnaireQuestion", Schema = "Survey", Lazy = true)]
    public partial class QuestionnaireQuestion_DAO : DB_2AM<QuestionnaireQuestion_DAO>
    {
        private int _key;

        private int _sequence;

        private IList<QuestionnaireQuestionAnswer_DAO> _questionnaireQuestionAnswers;

        private GeneralStatus_DAO _generalStatus;

        private Question_DAO _question;

        private Questionnaire_DAO _questionnaire;

        [PrimaryKey(PrimaryKeyType.Native, "QuestionnaireQuestionKey", ColumnType = "Int32")]
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

        [HasMany(typeof(QuestionnaireQuestionAnswer_DAO), ColumnKey = "QuestionnaireQuestionKey", Table = "QuestionnaireQuestionAnswer")]
        public virtual IList<QuestionnaireQuestionAnswer_DAO> QuestionAnswers
        {
            get
            {
                return this._questionnaireQuestionAnswers;
            }
            set
            {
                this._questionnaireQuestionAnswers = value;
            }
        }

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        [BelongsTo("QuestionKey", NotNull = true)]
        public virtual Question_DAO Question
        {
            get
            {
                return this._question;
            }
            set
            {
                this._question = value;
            }
        }

        [BelongsTo("QuestionnaireKey", NotNull = true)]
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
    }
}