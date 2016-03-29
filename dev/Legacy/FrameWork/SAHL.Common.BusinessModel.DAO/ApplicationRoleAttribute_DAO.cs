using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferRoleAttribute", Schema = "dbo", Lazy = true)]
    public partial class ApplicationRoleAttribute_DAO : DB_2AM<ApplicationRoleAttribute_DAO>
    {
        private int _key;

        private ApplicationRole_DAO _offerRole;

        private ApplicationRoleAttributeType_DAO _offerRoleAttributeType;

        [PrimaryKey(PrimaryKeyType.Native, "OfferRoleAttributeKey", ColumnType = "Int32")]
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

        [BelongsTo("OfferRoleKey", NotNull = true)]
        [ValidateNonEmpty("Offer Role is a mandatory field")]
        public virtual ApplicationRole_DAO OfferRole
        {
            get
            {
                return this._offerRole;
            }
            set
            {
                this._offerRole = value;
            }
        }

        [BelongsTo("OfferRoleAttributeTypeKey", NotNull = true)]
        [ValidateNonEmpty("Offer Role Attribute Type is a mandatory field")]
        public virtual ApplicationRoleAttributeType_DAO OfferRoleAttributeType
        {
            get
            {
                return this._offerRoleAttributeType;
            }
            set
            {
                this._offerRoleAttributeType = value;
            }
        }
    }
}