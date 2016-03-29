using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("DocumentVersion", Schema = "dbo", Lazy = true)]
    public partial class DocumentVersion_DAO : DB_2AM<DocumentVersion_DAO>
    {
        private string _version;

        private System.DateTime _effectiveDate;

        private bool _activeIndicator;

        private int _Key;

        //private IList<LoanAgreement_DAO> _loanAgreements;

        private DocumentType_DAO _documentType;

        [Property("Version", ColumnType = "String", NotNull = true, Length = 10)]
        [ValidateNonEmpty("Version is a mandatory field")]
        public virtual string Version
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
        [ValidateNonEmpty("Effective Date is a mandatory field")]
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

        [Property("ActiveIndicator", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Active Indicator is a mandatory field")]
        public virtual bool ActiveIndicator
        {
            get
            {
                return this._activeIndicator;
            }
            set
            {
                this._activeIndicator = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "DocumentVersionKey", ColumnType = "Int32")]
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

        // [HasMany(typeof(LoanAgreement_DAO), ColumnKey = "DocumentVersionKey", Table = "LoanAgreement")]
        //public virtual IList<LoanAgreement_DAO> LoanAgreements
        //{
        //    get
        //    {
        //        return this._loanAgreements;
        //    }
        //    set
        //    {
        //        this._loanAgreements = value;
        //    }
        //}

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