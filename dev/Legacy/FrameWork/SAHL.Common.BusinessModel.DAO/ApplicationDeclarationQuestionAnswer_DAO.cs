using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferDeclarationQuestionAnswer", Schema = "dbo", Lazy = true)]
    public partial class ApplicationDeclarationQuestionAnswer_DAO : DB_2AM<ApplicationDeclarationQuestionAnswer_DAO>
    {
        private int _key;

        private ApplicationDeclarationAnswer_DAO _applicationDeclarationAnswer;

        private ApplicationDeclarationQuestion_DAO _applicationDeclarationQuestion;

        [PrimaryKey(PrimaryKeyType.Native, "OfferDeclarationQuestionAnswerKey", ColumnType = "Int32")]
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

        [BelongsTo("OfferDeclarationAnswerKey", NotNull = true)]
        [ValidateNonEmpty("Application Declaration Answer is a mandatory field")]
        public virtual ApplicationDeclarationAnswer_DAO ApplicationDeclarationAnswer
        {
            get
            {
                return this._applicationDeclarationAnswer;
            }
            set
            {
                this._applicationDeclarationAnswer = value;
            }
        }

        [BelongsTo("OfferDeclarationQuestionKey", NotNull = true)]
        [ValidateNonEmpty("Application Declaration Question is a mandatory field")]
        public virtual ApplicationDeclarationQuestion_DAO ApplicationDeclarationQuestion
        {
            get
            {
                return this._applicationDeclarationQuestion;
            }
            set
            {
                this._applicationDeclarationQuestion = value;
            }
        }
    }
}