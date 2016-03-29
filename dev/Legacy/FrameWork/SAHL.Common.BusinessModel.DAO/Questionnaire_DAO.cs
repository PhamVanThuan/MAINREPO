using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Questionnaire", Schema = "Survey", Lazy = true)]
    public partial class Questionnaire_DAO : DB_2AM<Questionnaire_DAO>
    {
        private int _key;

        private string _description;

        private int _businessAreaGenericKey;

        private GenericKeyType_DAO _businessAreaGenericKeyType;

        private IList<QuestionnaireQuestion_DAO> _questionnaireQuestions;

        private IList<BusinessEventQuestionnaire_DAO> _businessEventQuestionnaires;

        private IList<QuestionnaireQuestionnaireEmail_DAO> _questionnaireQuestionnaireEmails;

        private GeneralStatus_DAO _generalStatus;

        [PrimaryKey(PrimaryKeyType.Native, "QuestionnaireKey", ColumnType = "Int32")]
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

        [Property("BusinessAreaGenericKey", ColumnType = "Int32", NotNull = true)]
        public virtual int BusinessAreaGenericKey
        {
            get
            {
                return this._businessAreaGenericKey;
            }
            set
            {
                this._businessAreaGenericKey = value;
            }
        }

        [BelongsTo("BusinessAreaGenericKeyTypeKey", NotNull = true)]
        public virtual GenericKeyType_DAO BusinessAreaGenericKeyType
        {
            get
            {
                return this._businessAreaGenericKeyType;
            }
            set
            {
                this._businessAreaGenericKeyType = value;
            }
        }

        [HasMany(typeof(QuestionnaireQuestion_DAO), ColumnKey = "QuestionnaireKey", Table = "QuestionnaireQuestion")]
        public virtual IList<QuestionnaireQuestion_DAO> Questions
        {
            get
            {
                return this._questionnaireQuestions;
            }
            set
            {
                this._questionnaireQuestions = value;
            }
        }

        [HasMany(typeof(BusinessEventQuestionnaire_DAO), ColumnKey = "QuestionnaireKey", Table = "BusinessEventQuestionnaire")]
        public virtual IList<BusinessEventQuestionnaire_DAO> BusinessEventQuestionnaires
        {
            get
            {
                return this._businessEventQuestionnaires;
            }
            set
            {
                this._businessEventQuestionnaires = value;
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

        [HasMany(typeof(QuestionnaireQuestionnaireEmail_DAO), ColumnKey = "QuestionnaireKey", Table = "QuestionnaireQuestionnaireEmail")]
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