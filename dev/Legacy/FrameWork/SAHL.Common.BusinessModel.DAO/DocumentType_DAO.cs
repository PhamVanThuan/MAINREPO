using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("DocumentType", Schema = "dbo", Lazy = true)]
    public partial class DocumentType_DAO : DB_2AM<DocumentType_DAO>
    {
        private string _description;

        private bool? _legalEntity;

        private int _Key;

        //private IList<AccountDocument_DAO> _accountDocuments;

        // todo: Uncomment when DocumentTypeGroup implemented
        //private IList<DocumentTypeGroup> _documentTypeGroups;

        private IList<DocumentType_DAO> _documentTypes;

        private IList<DocumentVersion_DAO> _documentVersions;

        private GenericKeyType_DAO _genericKeyType;

        [Property("Description", ColumnType = "String", Length = 255, NotNull = true)]
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

        [Property("LegalEntity", ColumnType = "Boolean")]
        public virtual bool? LegalEntity
        {
            get
            {
                return this._legalEntity;
            }
            set
            {
                this._legalEntity = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "DocumentTypeKey", ColumnType = "Int32")]
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

        [HasMany(typeof(DocumentType_DAO), ColumnKey = "DocumentTypeKey", Table = "DocumentType", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<DocumentType_DAO> DocumentTypes
        {
            get
            {
                return this._documentTypes;
            }
            set
            {
                this._documentTypes = value;
            }
        }

        [HasMany(typeof(DocumentVersion_DAO), ColumnKey = "DocumentTypeKey", Table = "DocumentVersion", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<DocumentVersion_DAO> DocumentVersions
        {
            get
            {
                return this._documentVersions;
            }
            set
            {
                this._documentVersions = value;
            }
        }

        [BelongsTo("GenericKeyTypeKey", NotNull = false)]
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