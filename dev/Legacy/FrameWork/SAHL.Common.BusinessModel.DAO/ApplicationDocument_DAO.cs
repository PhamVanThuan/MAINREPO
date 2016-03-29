using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferDocument", Schema = "dbo", Lazy = true)]
    public partial class ApplicationDocument_DAO : DB_2AM<ApplicationDocument_DAO>
    {
        private System.DateTime? _documentReceivedDate;

        private string _documentReceivedBy;

        private int _key;

        private IList<ApplicationDocumentReference_DAO> _applicationDocumentReferences;

        private Application_DAO _application;

        private DocumentType_DAO _documentType;

        private string _description;

        private int _genericKey;

        [Property("DocumentReceivedDate", NotNull = false)]
        public virtual System.DateTime? DocumentReceivedDate
        {
            get
            {
                return this._documentReceivedDate;
            }
            set
            {
                this._documentReceivedDate = value;
            }
        }

        [Property("DocumentReceivedBy", ColumnType = "String", NotNull = false)]
        public virtual string DocumentReceivedBy
        {
            get
            {
                return this._documentReceivedBy;
            }
            set
            {
                this._documentReceivedBy = value;
            }
        }

        [Property("Description", ColumnType = "String", NotNull = false)]
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

        [Property("GenericKey", ColumnType = "Int32", NotNull = false)]
        public virtual int GenericKey
        {
            get
            {
                return _genericKey;
            }
            set
            {
                _genericKey = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OfferDocumentKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ApplicationDocumentReference_DAO), ColumnKey = "OfferDocumentKey", Table = "OfferDocumentReference", Lazy = true)]
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

        [BelongsTo("OfferKey", NotNull = true)]
        [ValidateNonEmpty("Application is a mandatory field")]
        public virtual Application_DAO Application
        {
            get
            {
                return this._application;
            }
            set
            {
                this._application = value;
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
    }
}