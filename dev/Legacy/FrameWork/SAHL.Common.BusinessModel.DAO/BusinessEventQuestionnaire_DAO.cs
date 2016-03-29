using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("BusinessEventQuestionnaire", Schema = "Survey", Lazy = true)]
    public partial class BusinessEventQuestionnaire_DAO : DB_2AM<BusinessEventQuestionnaire_DAO>
    {
        private int _key;

        private BusinessEvent_DAO _businessEvent;

        private Questionnaire_DAO _questionnaire;

        [PrimaryKey(PrimaryKeyType.Native, "BusinessEventQuestionnaireKey", ColumnType = "Int32")]
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

        [BelongsTo("BusinessEventKey")]
        public virtual BusinessEvent_DAO BusinessEvent
        {
            get
            {
                return this._businessEvent;
            }
            set
            {
                this._businessEvent = value;
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