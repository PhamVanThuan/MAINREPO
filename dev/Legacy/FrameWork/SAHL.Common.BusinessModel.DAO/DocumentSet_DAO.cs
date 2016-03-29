using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("DocumentSet", Schema = "dbo", Lazy = true)]
    public partial class DocumentSet_DAO : DB_2AM<DocumentSet_DAO>
    {
        private string _description;

        private int _key;

        private IList<DocumentSetConfig_DAO> _documentSetConfigs;

        private ApplicationType_DAO _applicationType;

        private OriginationSourceProduct_DAO _originationSourceProduct;

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

        [PrimaryKey(PrimaryKeyType.Native, "DocumentSetKey", ColumnType = "Int32")]
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

        [HasMany(typeof(DocumentSetConfig_DAO), ColumnKey = "DocumentSetKey", Table = "DocumentSetConfig")]
        public virtual IList<DocumentSetConfig_DAO> DocumentSetConfigs
        {
            get
            {
                return this._documentSetConfigs;
            }
            set
            {
                this._documentSetConfigs = value;
            }
        }

        [BelongsTo("OfferTypeKey", NotNull = true)]
        [ValidateNonEmpty("Application Type is a mandatory field")]
        public virtual ApplicationType_DAO ApplicationType
        {
            get
            {
                return this._applicationType;
            }
            set
            {
                this._applicationType = value;
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