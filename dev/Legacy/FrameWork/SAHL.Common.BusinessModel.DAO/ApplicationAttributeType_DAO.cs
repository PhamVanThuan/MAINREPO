using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("OfferAttributeType", Schema = "dbo", Lazy = true)]
    public partial class ApplicationAttributeType_DAO : DB_2AM<ApplicationAttributeType_DAO>
    {
        private string _description;

        private bool _isGeneric;

        private int _key;

        private bool _userEditable;

        private IList<OriginationSourceProduct_DAO> _originationSourceProducts;

        private ApplicationAttributeTypeGroup_DAO _applicationAttributeTypeGroup;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "OfferAttributeTypeKey", ColumnType = "Int32")]
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

        [Property("IsGeneric", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Is Generic is a mandatory field")]
        public virtual bool IsGeneric
        {
            get
            {
                return this._isGeneric;
            }
            set
            {
                this._isGeneric = value;
            }
        }

        [HasAndBelongsToMany(typeof(OriginationSourceProduct_DAO), ColumnRef = "OriginationSourceProductKey", ColumnKey = "OfferAttributeTypeKey", Lazy = true, Schema = "dbo", Table = "OriginationSourceProductOfferAttributeType")]
        public virtual IList<OriginationSourceProduct_DAO> OriginationSourceProducts
        {
            get
            {
                return this._originationSourceProducts;
            }
            set
            {
                this._originationSourceProducts = value;
            }
        }

        [BelongsTo("OfferAttributeTypeGroupKey")]
        public virtual ApplicationAttributeTypeGroup_DAO ApplicationAttributeTypeGroup
        {
            get
            {
                return this._applicationAttributeTypeGroup;
            }
            set
            {
                this._applicationAttributeTypeGroup = value;
            }
        }

        [Property("UserEditable", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("UserEditable is a mandatory field")]
        public virtual bool UserEditable
        {
            get
            {
                return this._userEditable;
            }
            set
            {
                this._userEditable = value;
            }
        }
    }
}