using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferDocumentReference", Schema = "dbo", Lazy = true)]
    public partial class ApplicationDocumentReference_DAO : DB_2AM<ApplicationDocumentReference_DAO>
    {
        private int _genericKey;

        private int _key;

        private DocumentTypeReferenceObject_DAO _documentTypeReferenceObject;

        private ApplicationDocument_DAO _applicationDocument;

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

        [PrimaryKey(PrimaryKeyType.Native, "OfferDocumentReferenceKey", ColumnType = "Int32")]
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

        [BelongsTo("DocumentTypeReferenceObjectKey", NotNull = true)]
        [ValidateNonEmpty("Document Type Reference Object is a mandatory field")]
        public virtual DocumentTypeReferenceObject_DAO DocumentTypeReferenceObject
        {
            get
            {
                return this._documentTypeReferenceObject;
            }
            set
            {
                this._documentTypeReferenceObject = value;
            }
        }

        [BelongsTo("OfferDocumentKey", NotNull = true)]
        [ValidateNonEmpty("Application Document is a mandatory field")]
        public virtual ApplicationDocument_DAO ApplicationDocument
        {
            get
            {
                return this._applicationDocument;
            }
            set
            {
                this._applicationDocument = value;
            }
        }
    }
}