using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("OfferDeclarationQuestionAnswerConfiguration", Schema = "dbo", Lazy = true)]
    public partial class ApplicationDeclarationQuestionAnswerConfiguration_DAO : DB_2AM<ApplicationDeclarationQuestionAnswerConfiguration_DAO>
    {
        private int _key;

        private int _genericKey;

        private GenericKeyType_DAO _genericKeyType;

        private ApplicationDeclarationQuestion_DAO _applicationDeclarationQuestion;

        private LegalEntityType_DAO _legalEntityType;

        private OriginationSourceProduct_DAO _originationSourceProduct;

        [PrimaryKey(PrimaryKeyType.Native, "OfferDeclarationQuestionAnswerConfigurationKey", ColumnType = "Int32")]
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

        [Property("GenericKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Generic Key is a mandatory field")]
        public virtual int GenericKey
        {
            get
            {
                return this._genericKey;
            }
            set
            {
                this._genericKey = value;
            }
        }

        [BelongsTo("GenericKeyTypeKey", NotNull = true)]
        [ValidateNonEmpty("Generic Key Type Key is a mandatory field")]
        public virtual GenericKeyType_DAO GenericKeyType
        {
            get
            {
                return this._genericKeyType;
            }
            set
            {
                this._genericKeyType = value;
            }
        }

        [BelongsTo("OriginationSourceProductKey", NotNull = true)]
        [ValidateNonEmpty("Origination Source Product is a mandatory field")]
        public virtual OriginationSourceProduct_DAO OriginationSourceProduct
        {
            get
            {
                return this._originationSourceProduct;
            }
            set
            {
                this._originationSourceProduct = value;
            }
        }

        [BelongsTo("LegalEntityTypeKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity Type is a mandatory field")]
        public virtual LegalEntityType_DAO LegalEntityType
        {
            get
            {
                return this._legalEntityType;
            }
            set
            {
                this._legalEntityType = value;
            }
        }
    }
}