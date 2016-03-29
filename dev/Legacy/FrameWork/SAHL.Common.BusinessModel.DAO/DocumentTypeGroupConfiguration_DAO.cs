using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("DocumentTypeGroupConfiguration", Schema = "dbo", Lazy = true)]
    public partial class DocumentTypeGroupConfiguration_DAO : DB_2AM<DocumentTypeGroupConfiguration_DAO>
    {
        private int _key;

        private DocumentType_DAO _documentType;

        private DocumentGroup_DAO _documentGroup;

        private OriginationSourceProduct_DAO _originationSourceProduct;

        [PrimaryKey(PrimaryKeyType.Native, "DocumentTypeGroupConfigurationKey", ColumnType = "Int32")]
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

        [BelongsTo("DocumentTypeKey", NotNull = true)]
        [ValidateNonEmpty("Document Type is a mandatory field")]
        public virtual DocumentType_DAO DocumentType
        {
            get
            {
                return this._documentType;
            }
            set
            {
                this._documentType = value;
            }
        }

        [BelongsTo("DocumentGroupKey", NotNull = true)]
        [ValidateNonEmpty("Document Group is a mandatory field")]
        public virtual DocumentGroup_DAO DocumentGroup
        {
            get
            {
                return this._documentGroup;
            }
            set
            {
                this._documentGroup = value;
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
    }
}