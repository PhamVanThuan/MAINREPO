using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("DocumentTypeReferenceObject", Schema = "dbo", Lazy = true)]
    public partial class DocumentTypeReferenceObject_DAO : DB_2AM<DocumentTypeReferenceObject_DAO>
    {
        private int _key;

        private IList<ApplicationDocumentReference_DAO> _applicationDocumentReferences;

        private DocumentType_DAO _documentType;

        private GenericKeyType_DAO _genericKeyType;

        [PrimaryKey(PrimaryKeyType.Native, "DocumentTypeReferenceObjectKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ApplicationDocumentReference_DAO), ColumnKey = "DocumentTypeReferenceObjectKey", Table = "OfferDocumentReference", Lazy = true)]
        public virtual IList<ApplicationDocumentReference_DAO> ApplicationDocumentReferences
        {
            get
            {
                return this._applicationDocumentReferences;
            }
            set
            {
                this._applicationDocumentReferences = value;
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

        [BelongsTo("GenericKeyTypeKey", NotNull = true)]
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
    }
}