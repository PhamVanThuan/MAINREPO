using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Question", Schema = "Survey", Lazy = true)]
    public partial class Question_DAO : DB_2AM<Question_DAO>
    {
        private int _key;

        private string _description;

        private GeneralStatus_DAO _generalStatus;

        private IList<QuestionnaireQuestion_DAO> _questionnaireQuestions;

        [PrimaryKey(PrimaryKeyType.Native, "QuestionKey", ColumnType = "Int32")]
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

        [HasMany(typeof(QuestionnaireQuestion_DAO), ColumnKey = "QuestionKey", Table = "QuestionnaireQuestion")]
        public virtual IList<QuestionnaireQuestion_DAO> QuestionnaireQuestions
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
    }
}