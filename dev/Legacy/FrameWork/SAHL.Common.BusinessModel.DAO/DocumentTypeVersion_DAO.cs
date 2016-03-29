using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;


namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("DocumentTypeVersion",  Schema = "dbo")]
    public partial class DocumentTypeVersion_DAO : DB_2AM<DocumentTypeVersion_DAO>
    {

        private int _version;

        private System.DateTime _effectiveDate;

        private int _Key;
        // todo: Uncomment when OfferDocument implemented
        //private IList<OfferDocument> _applicationDocuments;

        private DocumentType_DAO _documentType;

        private GeneralStatus_DAO _generalStatus;

        [Property("Version", ColumnType = "Int32", NotNull = true)]
        public virtual int Version
        {
            get
            {
                return this._version;
            }
            set
            {
                this._version = value;
            }
        }

        [Property("EffectiveDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime EffectiveDate
        {
            get
            {
                return this._effectiveDate;
            }
            set
            {
                this._effectiveDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "DocumentTypeVersionKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }
        // todo: Uncomment when OfferDocument is implemented
        //[HasMany(typeof(OfferDocument), ColumnKey = "DocumentTypeVersionKey", Table = "OfferDocument")]
        //public virtual IList<OfferDocument> ApplicationDocuments
        //{
        //    get
        //    {
        //        return this._applicationDocuments;
        //    }
        //    set
        //    {
        //        this._applicationDocuments = value;
        //    }
        //}

        [BelongsTo("DocumentTypeKey", NotNull = true)]
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
    }
}
