using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("OfferDeclarationAnswer", Schema = "dbo", Lazy = true)]
    public partial class ApplicationDeclarationAnswer_DAO : DB_2AM<ApplicationDeclarationAnswer_DAO>
    {
        private string _description;

        private int _key;

        private IList<ApplicationDeclaration_DAO> _applicationDeclarations;

        private IList<ApplicationDeclarationQuestionAnswer_DAO> _applicationDeclarationQuestionAnswers;

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
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

        [PrimaryKey(PrimaryKeyType.Assigned, "OfferDeclarationAnswerKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ApplicationDeclaration_DAO), ColumnKey = "OfferDeclarationAnswerKey", Table = "OfferDeclaration", Lazy = true)]
        public virtual IList<ApplicationDeclaration_DAO> ApplicationDeclarations
        {
            get
            {
                return this._applicationDeclarations;
            }
            set
            {
                this._applicationDeclarations = value;
            }
        }

        [HasMany(typeof(ApplicationDeclarationQuestionAnswer_DAO), ColumnKey = "OfferDeclarationAnswerKey", Table = "OfferDeclarationQuestionAnswer", Lazy = true)]
        public virtual IList<ApplicationDeclarationQuestionAnswer_DAO> ApplicationDeclarationQuestionAnswers
        {
            get
            {
                return this._applicationDeclarationQuestionAnswers;
            }
            set
            {
                this._applicationDeclarationQuestionAnswers = value;
            }
        }
    }
}