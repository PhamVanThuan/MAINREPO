using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("DocumentSetConfig", Schema = "dbo", Lazy = true)]
    public partial class DocumentSetConfig_DAO : DB_2AM<DocumentSetConfig_DAO>
    {
        private int _key;

        private DocumentSet_DAO _documentSet;

        private DocumentType_DAO _documentType;

        private RuleItem_DAO _ruleItem;

        [PrimaryKey(PrimaryKeyType.Native, "DocumentSetConfigKey", ColumnType = "Int32")]
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

        [BelongsTo("DocumentSetKey", NotNull = true)]
        [ValidateNonEmpty("Document Set is a mandatory field")]
        public virtual DocumentSet_DAO DocumentSet
        {
            get
            {
                return this._documentSet;
            }
            set
            {
                this._documentSet = value;
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

        [BelongsTo("RuleItemKey", NotNull = true)]
        [ValidateNonEmpty("Rule Item is a mandatory field")]
        public virtual RuleItem_DAO RuleItem
        {
            get
            {
                return this._ruleItem;
            }
            set
            {
                this._ruleItem = value;
            }
        }
    }
}