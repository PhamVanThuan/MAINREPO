using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ExternalRoleDeclaration", Schema = "dbo", Lazy = true)]
    public partial class ExternalRoleDeclaration_DAO : DB_2AM<ExternalRoleDeclaration_DAO>
    {
        private System.DateTime? _externalRoleDeclarationDate;

        private int _key;

        private ApplicationDeclarationAnswer_DAO _applicationDeclarationAnswer;

        private ApplicationDeclarationQuestion_DAO _applicationDeclarationQuestion;

        private ExternalRole_DAO _externalRole;

        [BelongsTo("ExternalRoleKey", NotNull = true)]
        [ValidateNonEmpty("External Role is a mandatory field")]
        public virtual ExternalRole_DAO ExternalRole
        {
            get
            {
                return this._externalRole;
            }
            set
            {
                this._externalRole = value;
            }
        }

        [Property("ExternalRoleDeclarationDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ExternalRoleDeclarationDate
        {
            get
            {
                return this._externalRoleDeclarationDate;
            }
            set
            {
                this._externalRoleDeclarationDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ExternalRoleDeclarationKey", ColumnType = "Int32")]
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