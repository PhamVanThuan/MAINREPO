using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OriginationSourceIcon", Schema = "dbo", Lazy = true)]
    public partial class OriginationSourceIcon_DAO : DB_2AM<OriginationSourceIcon_DAO>
    {
        private int _originationSourceKey;

        private string _icon;

        private int _key;

        [Property("OriginationSourceKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Origination Source is a mandatory field")]
        public virtual int OriginationSourceKey
        {
            get
            {
                return this._originationSourceKey;
            }
            set
            {
                this._originationSourceKey = value;
            }
        }

        [Property("Icon", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Icon is a mandatory field")]
        public virtual string Icon
        {
            get
            {
                return this._icon;
            }
            set
            {
                this._icon = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OriginationSourceIconKey", ColumnType = "Int32")]
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
    }
}