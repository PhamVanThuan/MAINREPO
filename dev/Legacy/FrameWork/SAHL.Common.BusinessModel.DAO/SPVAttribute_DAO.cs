using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("SPVAttribute", Schema = "spv", Lazy = true)]
    public partial class SPVAttribute_DAO : DB_2AM<SPVAttribute_DAO>
    {
        private int _key;
        private SPV_DAO _spv;
        private SPVAttributeType_DAO _spvAttributeType;
        private string _value;

        [PrimaryKey(PrimaryKeyType.Native, "SPVAttributeKey", ColumnType = "Int32")]
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

        [BelongsTo("SPVKey", NotNull = true)]
        [ValidateNonEmpty("SPV is a mandatory field")]
        public virtual SPV_DAO SPV
        {
            get
            {
                return this._spv;
            }
            set
            {
                this._spv = value;
            }
        }

        [BelongsTo("SPVAttributeTypeKey", NotNull = true)]
        [ValidateNonEmpty("SPVAttributeTypeKeyType is a mandatory field")]
        public virtual SPVAttributeType_DAO SPVAttributeType
        {
            get
            {
                return this._spvAttributeType;
            }
            set
            {
                this._spvAttributeType = value;
            }
        }

        [Property("Value")]
        public virtual string Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }
    }
}