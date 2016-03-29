using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Answer", Schema = "Survey", Lazy = true)]
    public partial class Answer_DAO : DB_2AM<Answer_DAO>
    {
        private int _key;

        private string _description;

        private IList<AnswerImage_DAO> _answerImages;

        private IList<QuestionnaireQuestionAnswer_DAO> _questionnaireQuestionAnswers;

        private GeneralStatus_DAO _generalStatus;

        private AnswerType_DAO _answerType;

        [PrimaryKey(PrimaryKeyType.Native, "AnswerKey", ColumnType = "Int32")]
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

        [Property("Description", ColumnType = "String", NotNull = true)]
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

        [HasMany(typeof(AnswerImage_DAO), ColumnKey = "AnswerKey", Table = "AnswerImage")]
        public virtual IList<AnswerImage_DAO> AnswerImages
        {
            get
            {
                return this._answerImages;
            }
            set
            {
                this._answerImages = value;
            }
        }

        [HasMany(typeof(QuestionnaireQuestionAnswer_DAO), ColumnKey = "AnswerKey", Table = "QuestionnaireQuestionAnswer")]
        public virtual IList<QuestionnaireQuestionAnswer_DAO> QuestionnaireQuestionAnswers
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

        [BelongsTo("AnswerTypeKey", NotNull = true)]
        public virtual AnswerType_DAO AnswerType
        {
            get
            {
                return this._answerType;
            }
            set
            {
                this._answerType = value;
            }
        }
    }
}