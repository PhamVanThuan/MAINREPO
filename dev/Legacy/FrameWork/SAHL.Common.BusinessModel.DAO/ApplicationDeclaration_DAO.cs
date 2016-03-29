using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferDeclaration", Schema = "dbo", Lazy = true)]
    public partial class ApplicationDeclaration_DAO : DB_2AM<ApplicationDeclaration_DAO>
    {
        private System.DateTime? _applicationDeclarationDate;

        private int _key;

        private ApplicationDeclarationAnswer_DAO _applicationDeclarationAnswer;

        private ApplicationDeclarationQuestion_DAO _applicationDeclarationQuestion;

        private ApplicationRole_DAO _applicationRole;

        [BelongsTo("OfferRoleKey", NotNull = true)]
        [ValidateNonEmpty("Application Role is a mandatory field")]
        public virtual ApplicationRole_DAO ApplicationRole
        {
            get
            {
                return this._applicationRole;
            }
            set
            {
                this._applicationRole = value;
            }
        }

        [Property("OfferDeclarationDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ApplicationDeclarationDate
        {
            get
            {
                return this._applicationDeclarationDate;
            }
            set
            {
                this._applicationDeclarationDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OfferDeclarationKey", ColumnType = "Int32")]
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